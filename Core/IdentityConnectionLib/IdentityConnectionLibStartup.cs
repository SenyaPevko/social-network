using Core.Logic.Connections.Base;
using IdentityConnectionLib.Config.Models;
using IdentityConnectionLib.ResponseHandlers;
using IdentityConnectionLib.Services.Http;
using IdentityConnectionLib.Services.Identity;
using IdentityConnectionLib.Services.RabbitMq;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityConnectionLib
{
    /// <summary>
    /// Registrating Identity api connection services into di container
    /// </summary>
    public static class IdentityConnectionLibStartup
    {
        /// <summary>
        /// Add services to connect to the identity api
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityConnectionLibServices(this IServiceCollection services)
        {
            services.ConfigureConnections();
            services.AddScoped<IIdentityConnectionService, IdentityConnectionService>();
            services.AddScoped<IRabbitMqConnectionService, RabbitMqConnectionService>();
            services.AddScoped<IHttpConnectionService, HttpConnectionService>();
            services.AddScoped<IResponseHandler, ResponseHandler>();

            return services;
        }

        /// <summary>
        /// Add connections configuration  
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureConnections(this IServiceCollection services)
        {
            services.AddSingleton<IIdentityApiConnectionConfig, IdentityApiConnectionConfig>();
            services.AddSingleton<IConnectionConfiguration, IdentityApiConnectionConfig>();

            return services;
        }
    }
}