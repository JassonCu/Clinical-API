using Clinical.API.Extensions.Middleware;
using Clinical.API.HealthChecks;
using Clinical.Infraestructure.Extensions;
using Clinical.Persistence.Extensions;
using Clinical.UseCases.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, loggerConfiguration) =>
    {
        loggerConfiguration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentName()
            .Enrich.WithProcessId()
            .Enrich.WithThreadId()
            .Enrich.WithProperty("Application", "Clinical.API");

        // Console: human-readable in development, structured JSON in production (for log collectors).
        if (context.HostingEnvironment.IsDevelopment())
            loggerConfiguration.WriteTo.Console(
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {SourceContext} {Message:lj} {Properties:j}{NewLine}{Exception}");
        else
            loggerConfiguration.WriteTo.Console(new CompactJsonFormatter());

        // Rolling file, bounded so it can never fill the disk.
        loggerConfiguration.WriteTo.File(
            new CompactJsonFormatter(),
            "logs/clinical-.log",
            rollingInterval: RollingInterval.Day,
            rollOnFileSizeLimit: true,
            fileSizeLimitBytes: 50 * 1024 * 1024,
            retainedFileCountLimit: 30);

        // Centralized logging (opt-in): active only when a Seq server is configured.
        var seqUrl = context.Configuration["Serilog:SeqUrl"];
        if (!string.IsNullOrWhiteSpace(seqUrl))
            loggerConfiguration.WriteTo.Seq(seqUrl, apiKey: context.Configuration["Serilog:SeqApiKey"]);
    });

    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var secretKey = jwtSettings["SecretKey"];

    // Fail fast: never start without a valid signing key or connection string.
    // In production these must come from environment variables / a secret store, never from committed files.
    if (string.IsNullOrWhiteSpace(secretKey) || secretKey.Length < 32)
        throw new InvalidOperationException(
            "JwtSettings:SecretKey no está configurada o es demasiado corta (mínimo 32 caracteres). " +
            "Configúrela mediante la variable de entorno JwtSettings__SecretKey.");

    if (!builder.Environment.IsDevelopment() &&
        (secretKey.Contains("ChangeInProduction") || secretKey.StartsWith("DEV-ONLY")))
        throw new InvalidOperationException(
            "La SecretKey de ejemplo no puede usarse fuera de desarrollo. " +
            "Rote la clave y configúrela mediante la variable de entorno JwtSettings__SecretKey.");

    if (string.IsNullOrWhiteSpace(builder.Configuration.GetConnectionString("ClinicalConnection")))
        throw new InvalidOperationException(
            "ConnectionStrings:ClinicalConnection no está configurada. " +
            "Configúrela mediante la variable de entorno ConnectionStrings__ClinicalConnection.");

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

    builder.Services.AddAuthorization();

    var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("ClinicalPolicy", policy =>
        {
            if (builder.Environment.IsDevelopment())
                policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            else
                policy.WithOrigins(allowedOrigins).WithHeaders("Authorization", "Content-Type").WithMethods("GET", "POST", "PUT", "PATCH", "DELETE").AllowCredentials();
        });
    });

    builder.Services.AddRateLimiter(options =>
    {
        options.AddFixedWindowLimiter("auth", opt =>
        {
            opt.PermitLimit = 5;
            opt.Window = TimeSpan.FromMinutes(1);
            opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            opt.QueueLimit = 0;
        });
        options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
            RateLimitPartition.GetFixedWindowLimiter(
                ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                _ => new FixedWindowRateLimiterOptions { PermitLimit = 100, Window = TimeSpan.FromMinutes(1) }));
        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    });

    builder.Services.AddControllers();
    builder.Services.AddProblemDetails();
    builder.Services.AddOpenApi();

    builder.Services.AddInyectionInfrastructure();
    builder.Services.AddInyectionPersistence();
    builder.Services.AddInyectionApplication();

    // Trust the reverse proxy / load balancer so the real client IP (X-Forwarded-For)
    // reaches the rate limiter, and the scheme (X-Forwarded-Proto) is honored.
    // In fixed infrastructure, restrict KnownProxies/KnownNetworks to your proxy for stronger safety.
    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        options.KnownNetworks.Clear();
        options.KnownProxies.Clear();
    });

    builder.Services.AddHealthChecks()
        .AddCheck<DatabaseHealthCheck>("database");

    var app = builder.Build();

    app.UseForwardedHeaders();
    app.UseRateLimiter();

    app.Use(async (context, next) =>
    {
        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Append("X-Frame-Options", "DENY");
        context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
        context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
        context.Response.Headers.Append("Permissions-Policy", "geolocation=(), microphone=(), camera=()");
        if (!app.Environment.IsDevelopment())
        {
            context.Response.Headers.Append("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
            // The API only serves JSON; lock down resource loading and framing.
            context.Response.Headers.Append("Content-Security-Policy", "default-src 'none'; frame-ancestors 'none'; base-uri 'none'; form-action 'none'");
        }
        await next();
    });

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options.Title = "Clinical API";
            options.Theme = ScalarTheme.Purple;
        });
    }

    app.UseHttpsRedirection();
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        options.GetLevel = (httpContext, elapsed, ex) =>
            ex is not null || httpContext.Response.StatusCode >= 500 ? LogEventLevel.Error
            : httpContext.Response.StatusCode >= 400 ? LogEventLevel.Warning
            : httpContext.Request.Path.StartsWithSegments("/health") ? LogEventLevel.Verbose
            : LogEventLevel.Information;
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
            diagnosticContext.Set("ClientIp", httpContext.Connection.RemoteIpAddress?.ToString());
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent.ToString());
            // Attributes the request to a user for traceability. Bodies/headers are never logged (no PHI/credentials).
            if (httpContext.User.Identity?.IsAuthenticated == true)
            {
                diagnosticContext.Set("UserName", httpContext.User.Identity.Name);
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                             ?? httpContext.User.FindFirst("sub")?.Value;
                if (userId is not null)
                    diagnosticContext.Set("UserId", userId);
            }
        };
    });
    app.UseCors("ClinicalPolicy");
    app.UseAuthentication();
    app.UseAuthorization();
    app.AddMiddleware();
    app.MapControllers();
    app.MapHealthChecks("/health");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start.");
}
finally
{
    Log.CloseAndFlush();
}
