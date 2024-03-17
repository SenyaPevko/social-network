using Core.Logic.Connections.RabbitMqLogic.Connections.Models;
using Core.Logic.Connections.RabbitMqLogic.Connections.Services;
using Core.Logic.Connections.RabbitMqLogic.Geneartors.Ids;
using Core.Logic.Connections.RabbitMqLogic.Geneartors.QueueNames;
using Core.Logic.Connections.RabbitMqLogic.Requests.Services;
using Core.Logic.Connections.RabbitMqLogic.Responses.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Logic.Connections.RabbitMqLogic
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
            services.AddSingleton<RabbitMqConnectionData, RabbitMqConnectionData>();
            services.AddSingleton<IRabbitMqConnectionService, RabbitMqConnectionService>();
            services.AddSingleton<IRabbitMqRequestService, RabbitMqRequestService>();
            services.AddSingleton<IRabbitMqResponseService, RabbitMqResponseService>();
            services.AddGenerators();

            return services;
        }

        /// <summary>
        /// Adding generators for RabbitMq services
        /// </summary>
        private static IServiceCollection AddGenerators(this IServiceCollection services)
        {
            services.AddSingleton<IIdGenerator, IdGenerator>();
            services.AddSingleton<IQueueNameGenerator, QueueNameGenerator>();

            return services;
        }
    }
}
