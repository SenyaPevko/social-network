using Core.TraceLogic.TraceWriters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Core.TraceIdLogic.TraceIdAccessors
{
    public static class StartupTraceId
    {
        public static IServiceCollection TryAddTraceId(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<TraceIdAccessor>();
            serviceCollection
                .TryAddScoped<ITraceWriter>(provider => provider.GetRequiredService<TraceIdAccessor>());
            serviceCollection
                .TryAddScoped<ITraceReader>(provider => provider.GetRequiredService<TraceIdAccessor>());
            serviceCollection
                .TryAddScoped<ITraceIdAccessor>(provider => provider.GetRequiredService<TraceIdAccessor>());

            return serviceCollection;
        }
    }
}
