using Core.HttpLogic.HttpConnections.Services;
using Core.HttpLogic.HttpRequests.Services;
using Core.HttpLogic.Polly;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Core.HttpLogic
{
    /// <summary>
    /// Registrating http services into di container
    /// </summary>
    public static class HttpServiceStartup
    {
        /// <summary>
        /// Adding services to send requests by http protocol
        /// </summary>
        public static IServiceCollection AddHttpServices(this IServiceCollection services)
        {
            services
                .AddHttpContextAccessor()
                .AddHttpClient()
                .AddTransient<IHttpConnectionService, HttpConnectionService>();
            
            services.TryAddTransient<IHttpPolicy, HttpPolicy>();
            services.TryAddTransient<IHttpRequestService, HttpRequestService>();

            return services;
        }
    }
}
