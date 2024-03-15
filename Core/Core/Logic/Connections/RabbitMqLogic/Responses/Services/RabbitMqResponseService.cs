using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Core.Logic.Connections.RabbitMqLogic.Connections.Services;
using Newtonsoft.Json;
using Core.Logic.Connections.RabbitMqLogic.Geneartors.QueueNames;
using RabbitMQ.Client.Exceptions;

namespace Core.Logic.Connections.RabbitMqLogic.Responses.Services
{
    /// <inheritdoc />
    public class RabbitMqResponseService : IRabbitMqResponseService
    {
        private readonly IModel channel;
        private readonly IRabbitMqConnectionService connectionService;
        private readonly IQueueNameGenerator queueNameGenerator;

        public RabbitMqResponseService(IRabbitMqConnectionService connectionService, IQueueNameGenerator queueNameGenerator)
        {
            this.connectionService = connectionService;
            this.queueNameGenerator = queueNameGenerator;
            channel = connectionService.CreateChannel();
        }

        /// <inheritdoc />
        public void StartListeningForRequests<TRequest, TResponse>(Func<TRequest, Task<TResponse>> handleRequest)
        {
            var requestQueueName = queueNameGenerator.GenerateRequestQueueName<TRequest>();
            var consumer = new EventingBasicConsumer(channel);
            channel.QueueDeclare(queue: requestQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            consumer.Received += (model, ea) => HandleRequestReceived(model, ea, handleRequest);
            channel.BasicConsume(queue: requestQueueName, autoAck: false, consumer: consumer);
        }

        /// <summary>
        /// Handles the received request from the client
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ea"></param>
        /// <param name="handleRequest"></param>
        private async void HandleRequestReceived<TRequest, TResponse>(
            object model,
            BasicDeliverEventArgs ea,
            Func<TRequest, Task<TResponse>> handleRequest)
        {
            var responseQueueName = ea.BasicProperties.ReplyTo;
            var body = ea.Body.ToArray();
            var requestJson = Encoding.UTF8.GetString(body);
            var request = JsonConvert.DeserializeObject<TRequest>(requestJson);
            var response = await handleRequest.Invoke(request);
            SendResponse(ea.BasicProperties.CorrelationId, response, responseQueueName, responseQueueName, responseQueueName);
            channel.BasicAck(ea.DeliveryTag, false);
        }

        /// <summary>
        /// Sending response back to the client
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="response"></param>
        /// <param name="responseQueueName"></param>
        private void SendResponse<TMessage>(
            string correlationId,
            TMessage response,
            string responseQueueName,
            string responseExchange,
            string responseRoutingKey)
        {
            var props = CreateBasicProperties(correlationId);
            connectionService.PublishMessage(response, props, responseExchange, responseRoutingKey, channel);
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
