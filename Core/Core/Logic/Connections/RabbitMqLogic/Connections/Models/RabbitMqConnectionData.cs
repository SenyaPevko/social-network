using Core.Logic.Connections.Base;

namespace Core.Logic.Connections.RabbitMqLogic.Connections.Models
{
    /// <summary>
    /// Data that is needed to connect to RabbitMq
    /// </summary>
    public class RabbitMqConnectionData
    {
        /// <summary>
        /// RabbitMq connection type
        /// </summary>
        public RabbitMqConnectionType ConnectionType { get; set; }

        /// <summary>
        /// AmqpUrl to connect to RabbitMq server
        /// </summary>
        public string AmqpUrl { get; }

        /// <summary>
        /// Host name to connect to RabbitMq
        /// </summary>
        public string HostName { get; }

        public RabbitMqConnectionData(IConnectionConfiguration connectionConfiguration)
        {
            ConnectionType = connectionConfiguration.RabbitMqConnectionType;
            AmqpUrl = connectionConfiguration.RabbitMqUrl;
            HostName = connectionConfiguration.RabbitMqHostName;
        }
    }
}
