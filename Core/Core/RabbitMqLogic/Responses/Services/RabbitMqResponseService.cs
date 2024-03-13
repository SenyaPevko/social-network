using Core.RabbitMqLogic.Connections;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace Core.RabbitMqLogic.Responses.Services
{
    /// <inheritdoc />
    public class RabbitMqResponseService : IRabbitMqResponseService
    {
        private readonly IModel channel;
        private readonly string exchangeName;
        private readonly string responseQueueName;
        private readonly IRabbitMqConnectionService connectionService;

        public RabbitMqResponseService(
            IRabbitMqConnectionService connectionService,
            string exchangeName,
            string responseQueueName)
        {
            this.connectionService = connectionService;
            this.exchangeName = exchangeName;
            this.responseQueueName = responseQueueName;
            channel = connectionService.CreateChannel();
        }

        /// <inheritdoc />
        public void StartListeningForRequests(Func<string, string> handleRequest)
        {
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);
            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName,
                               exchange: exchangeName,
                               routingKey: queueName);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) => HandleRequestReceived(model, ea, handleRequest);
            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        /// <summary>
        /// Handles the received request from the client
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ea"></param>
        /// <param name="handleRequest"></param>
        private void HandleRequestReceived(object model, BasicDeliverEventArgs ea, Func<string, string> handleRequest)
        {
            var body = ea.Body.ToArray();
            var request = Encoding.UTF8.GetString(body);
            var response = handleRequest.Invoke(request);
            SendResponse(ea.BasicProperties.CorrelationId, response);
        }

        /// <summary>
        /// Sending response back to the client
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="response"></param>
        private void SendResponse(string correlationId, string response)
        {
            var props = CreateBasicProperties(correlationId);
            connectionService.PublishMessage(response, props, exchangeName, responseQueueName, channel);
        }

        /// <summary>
        /// Creates basic properties to use when sending a message
        /// </summary>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        private IBasicProperties CreateBasicProperties(string correlationId)
        {
            var props = channel.CreateBasicProperties();
            props.CorrelationId = correlationId;

            return props;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            channel.Close();
        }
    }
}
