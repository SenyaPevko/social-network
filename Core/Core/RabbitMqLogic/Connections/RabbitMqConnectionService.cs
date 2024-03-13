using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Core.RabbitMqLogic.Connections
{
    /// <inheritdoc />
    public class RabbitMqConnectionService : IRabbitMqConnectionService, IDisposable
    {
        private readonly ConnectionFactory factory;
        private readonly IConnection connection;

        public RabbitMqConnectionService(string hostName)
        {
            factory = new ConnectionFactory() { HostName = hostName };
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
            string queueName,
            IModel channel)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: exchangeName);
            channel.BasicPublish(exchange: exchangeName, routingKey: queueName, basicProperties: props, body: body);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            connection.Close();
        }
    }
}
