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
        private readonly IRabbitMqConnectionService connectionService;

        public RabbitMqResponseService(IRabbitMqConnectionService connectionService)
        {
            this.connectionService = connectionService;
            channel = connectionService.CreateChannel();
        }

        /// <inheritdoc />
        public void StartListeningForRequests(
            Func<string, string> handleRequest,
            string requestQueueName,
            string responseQueueName)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) => HandleRequestReceived(model, ea, handleRequest, responseQueueName);
            channel.BasicConsume(queue: requestQueueName, autoAck: true, consumer: consumer);
        }

        /// <summary>
        /// Handles the received request from the client
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ea"></param>
        /// <param name="handleRequest"></param>
        /// <param name="responseQueueName"></param>
        private void HandleRequestReceived(
            object model,
            BasicDeliverEventArgs ea,
            Func<string, string> handleRequest,
            string responseQueueName)
        {
            var body = ea.Body.ToArray();
            var request = Encoding.UTF8.GetString(body);
            var response = handleRequest.Invoke(request);
            SendResponse(ea.BasicProperties.CorrelationId, response, responseQueueName);
        }

        /// <summary>
        /// Sending response back to the client
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="response"></param>
        /// <param name="responseQueueName"></param>
        private void SendResponse(string correlationId, string response, string responseQueueName)
        {
            var props = CreateBasicProperties(correlationId);
            connectionService.PublishMessage(response, props, responseQueueName, responseQueueName, channel);
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
