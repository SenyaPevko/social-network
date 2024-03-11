using IdentityConnectionLib.Config.Models;
using IdentityConnectionLib.ResponseHandlers;
using IdentityConnectionLib.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityConnectionLib
{
    public static class IdentityConnectionLibStartup
    {
        public static IServiceCollection AddIdentityConnectionLibServices(this IServiceCollection services)
        {
            services.ConfigureConnections();
            services.AddScoped<IIdentityConnectionService,IdentityConnectionService>();
            services.AddScoped<IResponseHandler, ResponseHandler>();
            
            return services;
        }

        public static IServiceCollection ConfigureConnections(this IServiceCollection services)
        {
           services.AddSingleton<IIdentityApiConnectionConfig, IdentityApiConnectionConfig>();

            return services;
        }
    }
}
