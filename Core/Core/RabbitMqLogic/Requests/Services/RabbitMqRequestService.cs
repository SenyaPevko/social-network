using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Collections.Concurrent;
using System.Text;
using Newtonsoft.Json;
using Core.RabbitMqLogic.Connections;
using Core.RabbitMqLogic.Geneartors.QueueNames;
using Core.RabbitMqLogic.Geneartors.Ids;

namespace Core.RabbitMqLogic.Requests.Services
{
    /// <inheritdoc />
    public class RabbitMqRequestService : IRabbitMqRequestService
    {
        private readonly IModel channel;
        private readonly string responseQueueName;
        private readonly IIdGenerator idGenerator;
        private readonly IQueueNameGenerator queueNameGenerator;
        private readonly IRabbitMqConnectionService connectionService;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> callbackMapper = new();

        public RabbitMqRequestService(
            IRabbitMqConnectionService connectionService,
            IQueueNameGenerator queueNameGenerator,
            IIdGenerator idGenerator)
        {
            this.connectionService = connectionService;
            this.queueNameGenerator = queueNameGenerator;
            this.idGenerator = idGenerator;
            channel = connectionService.CreateChannel();
            responseQueueName = DeclareResponseQueue();
            StartListeningForResponses();
        }

        /// <summary>
        /// Declares the response queue for receiving responses from the server
        /// </summary>
        /// <returns></returns>
        private string DeclareResponseQueue()
        {
            var responseQueueName = queueNameGenerator.GenerateResponseQueueName();
            return channel.QueueDeclare(queue: responseQueueName, durable: false, exclusive: true, autoDelete: true, arguments: null);
        }

        /// <summary>
        /// Starts listening for responses from the server
        /// </summary>
        private void StartListeningForResponses()
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += HandleResponseReceived;
            channel.BasicConsume(consumer: consumer, queue: responseQueueName, autoAck: true);
        }

        /// <summary>
        /// Handles the received response from the server
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ea"></param>
        private void HandleResponseReceived(object model, BasicDeliverEventArgs ea)
        {
            if (!callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out var tcs))
                return;
            var body = ea.Body.ToArray();
            var response = Encoding.UTF8.GetString(body);
            tcs.TrySetResult(response);
        }

        /// <inheritdoc />
        public async Task<TResponse> SendRequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);
            var correlationId = idGenerator.GenerateCorrelationId();
            var props = CreateBasicProperties(correlationId);
            var exchangeName = queueNameGenerator.GenerateRequestQueueName<TRequest>();
            var queueName = queueNameGenerator.GenerateRequestQueueName<TRequest>();
            connectionService.PublishMessage(request, props, exchangeName, queueName, channel);
            var response = await WaitForResponseAsync<TResponse>(correlationId, cancellationToken);

            return response;
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
            props.ReplyTo = responseQueueName;
            return props;
        }

        /// <summary>
        /// Waits for a response from the server
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<TResponse> WaitForResponseAsync<TResponse>(string correlationId, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<string>();
            callbackMapper.TryAdd(correlationId, tcs);
            cancellationToken.Register(() => callbackMapper.TryRemove(correlationId, out _));
            var response = await tcs.Task;

            return JsonConvert.DeserializeObject<TResponse>(response);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            channel.Close();
        }
    }

}