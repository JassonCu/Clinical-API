using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace Clinical.Observability
{
    /// <summary>
    /// Shared Serilog setup for every service (API, Web, …): configuration-driven levels, enrichers,
    /// human-readable console in Development / compact JSON in Production, a bounded rolling file,
    /// an always-on "Audit" level override, and an optional Seq sink. Keeps the two Program.cs in sync.
    /// </summary>
    public static class SerilogConfigurator
    {
        public static void Configure(
            LoggerConfiguration loggerConfiguration,
            IConfiguration configuration,
            IServiceProvider services,
            bool isDevelopment,
            string applicationName,
            string logFilePrefix)
        {
            loggerConfiguration
                .ReadFrom.Configuration(configuration)
                .ReadFrom.Services(services)
                // Audit trail must never be filtered out by the environment's minimum level.
                .MinimumLevel.Override("Audit", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentName()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .Enrich.WithProperty("Application", applicationName);

            // Console: human-readable in development, structured JSON in production (for log collectors).
            if (isDevelopment)
                loggerConfiguration.WriteTo.Console(
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {SourceContext} {Message:lj} {Properties:j}{NewLine}{Exception}");
            else
                loggerConfiguration.WriteTo.Console(new CompactJsonFormatter());

            // Rolling file, bounded so it can never fill the disk.
            loggerConfiguration.WriteTo.File(
                new CompactJsonFormatter(),
                $"logs/{logFilePrefix}-.log",
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true,
                fileSizeLimitBytes: 50 * 1024 * 1024,
                retainedFileCountLimit: 30);

            // Centralized logging (opt-in): active only when a Seq server is configured.
            var seqUrl = configuration["Serilog:SeqUrl"];
            if (!string.IsNullOrWhiteSpace(seqUrl))
                loggerConfiguration.WriteTo.Seq(seqUrl, apiKey: configuration["Serilog:SeqApiKey"]);
        }
    }
}
