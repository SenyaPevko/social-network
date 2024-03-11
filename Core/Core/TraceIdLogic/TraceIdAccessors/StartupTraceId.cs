using Core.TraceLogic.TraceReaders;
using Core.TraceLogic.TraceWriters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Core.TraceIdLogic.TraceIdAccessors;

/// <summary>
///     Registrating tracing services into di container
/// </summary>
public static class StartupTraceId
{
    /// <summary>
    ///     Adding services to trace id
    /// </summary>
    public static IServiceCollection AddTraceId(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<TraceIdAccessor>();
        serviceCollection
            .TryAddScoped<ITraceReader>(provider => provider.GetRequiredService<TraceIdAccessor>());
        serviceCollection
            .TryAddScoped<ITraceWriter>(provider => provider.GetRequiredService<TraceIdAccessor>());
        serviceCollection
            .TryAddScoped<ITraceIdAccessor>(provider => provider.GetRequiredService<TraceIdAccessor>());

        return serviceCollection;
    }
}