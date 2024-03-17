using Core.Logic.Connections.RabbitMqLogic.Connections.Models;

namespace Core.Logic.Connections.Base
{
    public interface IConnectionConfiguration
    {
        RabbitMqConnectionType RabbitMqConnectionType { get; }
        ConnectionType ConnectionType { get; }
        public string RabbitMqUrl { get; }
        public string RabbitMqHostName { get; }
    }
}
