using Clinical.Web.Extensions;
using Clinical.Web.Middleware;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

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
            .Enrich.WithProperty("Application", "Clinical.Web");

        // Console: human-readable in development, structured JSON in production (for log collectors).
        if (context.HostingEnvironment.IsDevelopment())
            loggerConfiguration.WriteTo.Console(
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {SourceContext} {Message:lj} {Properties:j}{NewLine}{Exception}");
        else
            loggerConfiguration.WriteTo.Console(new CompactJsonFormatter());

        // Rolling file, bounded so it can never fill the disk.
        loggerConfiguration.WriteTo.File(
            new CompactJsonFormatter(),
            "logs/clinical-web-.log",
            rollingInterval: RollingInterval.Day,
            rollOnFileSizeLimit: true,
            fileSizeLimitBytes: 50 * 1024 * 1024,
            retainedFileCountLimit: 30);

        // Centralized logging (opt-in): active only when a Seq server is configured.
        var seqUrl = context.Configuration["Serilog:SeqUrl"];
        if (!string.IsNullOrWhiteSpace(seqUrl))
            loggerConfiguration.WriteTo.Seq(seqUrl, apiKey: context.Configuration["Serilog:SeqApiKey"]);
    });

    builder.Services.AddControllersWithViews(options =>
    {
        options.Filters.Add(new Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute());
    });

    builder.Services.AddAntiforgery(options =>
    {
        options.Cookie.Name = "ClinicalWeb.XSRF";
        options.Cookie.HttpOnly = false;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.HeaderName = "X-XSRF-TOKEN";
    });

    builder.Services.AddSession(options =>
    {
        options.Cookie.Name = "ClinicalWeb.Session";
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.IdleTimeout = TimeSpan.FromMinutes(30);
    });

    builder.Services.AddCookieAuthentication();
    builder.Services.AddWebRateLimiting();
    builder.Services.AddApplicationServices(builder.Configuration);

    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
        app.UseHttpsRedirection();
    }

    app.UseStaticFiles();
    app.UseRouting();

    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        options.GetLevel = (httpContext, elapsed, ex) =>
            ex is not null || httpContext.Response.StatusCode >= 500 ? LogEventLevel.Error
            : httpContext.Response.StatusCode >= 400 ? LogEventLevel.Warning
            : httpContext.Request.Path.StartsWithSegments("/css")
              || httpContext.Request.Path.StartsWithSegments("/js")
              || httpContext.Request.Path.StartsWithSegments("/lib") ? LogEventLevel.Verbose
            : LogEventLevel.Information;
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("ClientIp", httpContext.Connection.RemoteIpAddress?.ToString());
            if (httpContext.User.Identity?.IsAuthenticated == true)
            {
                diagnosticContext.Set("UserName", httpContext.User.Identity.Name);
                var userId = httpContext.User.FindFirst("user_id")?.Value;
                if (userId is not null)
                    diagnosticContext.Set("UserId", userId);
            }
        };
    });

    app.UseRateLimiter();
    app.UseSession();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseMiddleware<TokenRefreshMiddleware>();
    app.UseMiddleware<SecurityHeadersMiddleware>();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Dashboard}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Clinical.Web failed to start.");
}
finally
{
    Log.CloseAndFlush();
}
