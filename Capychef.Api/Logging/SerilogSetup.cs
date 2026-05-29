using Serilog;
using Serilog.Events;

namespace Capychef.Api.Logging;

public class SerilogSetup
{
    public static void SetupSerilog(IWebHostEnvironment environment)
    {
        var minimumLevel = environment.IsDev() ? LogEventLevel.Debug : LogEventLevel.Warning;

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Is(minimumLevel)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.File(
                "../Logs/capychef-.log",
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true,
                retainedFileCountLimit: 14,
                outputTemplate:
                "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} " +
                "[{Level:u3}] " +
                "{CorrelationId} " +
                "{SourceContext} " +
                "{Message:lj}{NewLine}{Exception}"
            )
            .CreateLogger();
    }
}