using IdentityConnectionLib.ResponseHandlers;
using IdentityConnectionLib.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityConnectionLib
{
    public static class Startup
    {
        public static IServiceCollection AddIdentityConnectionLibServices(this IServiceCollection services)
        {
            services.AddScoped<IIdentityConnectionService, IdentityConnectionService>();
            services.AddScoped<IResponseHandler, ResponseHandler>();

            return services;
        }
    }
}
