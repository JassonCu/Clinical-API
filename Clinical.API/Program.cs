using Clinical.API.Extensions.Middleware;
using Clinical.Infraestructure.Extensions;
using Clinical.Persistence.Extensions;
using Clinical.UseCases.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using System.Text;
using System.Threading.RateLimiting;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
        .ReadFrom.Configuration(ctx.Configuration)
        .WriteTo.Console()
        .WriteTo.File("logs/clinical-.log", rollingInterval: RollingInterval.Day,
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"));

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

    builder.Services.AddHealthChecks();

    var app = builder.Build();

    app.UseRateLimiter();

    app.Use(async (context, next) =>
    {
        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Append("X-Frame-Options", "DENY");
        context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
        context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
        context.Response.Headers.Append("Permissions-Policy", "geolocation=(), microphone=(), camera=()");
        if (!app.Environment.IsDevelopment())
            context.Response.Headers.Append("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
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
    app.UseSerilogRequestLogging();
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
