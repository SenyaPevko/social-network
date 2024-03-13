using Core.RabbitMqLogic.Connections;
using Core.RabbitMqLogic.Geneartors.Ids;
using Core.RabbitMqLogic.Geneartors.QueueNames;
using Core.RabbitMqLogic.Requests.Services;
using Core.RabbitMqLogic.Responses.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core.RabbitMqLogic.Responses
{
    /// <summary>
    /// Registrating RabbitMq services into di container
    /// </summary>
    public static class RabbitMqServiceStartup
    {
        /// <summary>
        /// Adding services to publish messages to RabbitMq queues and to listen to them
        /// </summary>
        public static IServiceCollection AddRabbitMqServices(this IServiceCollection services)
        {
            services.AddTransient<IRabbitMqConnectionService, RabbitMqConnectionService>();
            services.AddTransient<IRabbitMqRequestService, RabbitMqRequestService>();
            services.AddTransient<IRabbitMqResponseService, RabbitMqResponseService>();
            AddGenerators(services);

            return services;
        }

        private static IServiceCollection AddGenerators(this IServiceCollection services)
        {
            services.AddTransient<IIdGenerator,  IdGenerator>();
            services.AddTransient<IQueueNameGenerator, QueueNameGenerator>();

            return services;
        }
    }
}
