using Core.Logic.Connections.RabbitMqLogic.Connections.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Core.Logic.Connections.RabbitMqLogic.Connections.Services
{
    /// <inheritdoc />
    public class RabbitMqConnectionService : IRabbitMqConnectionService
    {
        private readonly ConnectionFactory factory;
        private readonly IConnection connection;

        public RabbitMqConnectionService(RabbitMqConnectionData connectionData)
        {
            factory = connectionData.ConnectionType ==  RabbitMqConnectionType.Uri
                ? new ConnectionFactory() { Uri = new Uri(connectionData.AmqpUrl) } 
                : new ConnectionFactory() { HostName = connectionData.HostName };
            connection = factory.CreateConnection();
        }

        /// <inheritdoc />
        public IModel CreateChannel()
        {
            return connection.CreateModel();
        }

        /// <inheritdoc />
        public void PublishMessage<TMessage>(
            TMessage message,
            IBasicProperties props,
            string exchangeName,
            string routingKey,
            IModel channel)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, basicProperties: props, body: body);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            connection.Close();
        }
    }
}
