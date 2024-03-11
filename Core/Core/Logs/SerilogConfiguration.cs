using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace Core.Logs;

/// <summary>
///     Serilogger configuration
/// </summary>
public static class SerilogConfiguration
{
    /// <summary>
    ///     Add logger
    /// </summary>
    public static IServiceCollection AddLoggerServices(this IServiceCollection services)
    {
        return services
            .AddSingleton(Log.Logger);
    }

    /// <summary>
    ///     Get configuration
    /// </summary>
    public static LoggerConfiguration GetConfiguration(this LoggerConfiguration loggerConfiguration)
    {
        var logFormat =
            "{Timestamp:HH:mm:ss:ms} LEVEL:[{Level}] TRACE:|{TraceId}| THREAD:|{ThreadId}|{TenantId} {Message}{NewLine}{Exception}";

        return loggerConfiguration
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Information)
            .MinimumLevel.Is(LogEventLevel.Information)
            .Enrich.WithThreadId() // добавляет TraceId
            .Enrich.FromLogContext()
            .WriteTo.Async(option => { option.Console(LogEventLevel.Information, logFormat); });
    }
}