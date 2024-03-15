namespace Core.Logic.Connections.RabbitMqLogic.Responses.Services
{
    /// <summary>
    /// Service for servers to read message from their request queues and 
    /// publish messages to their queue using RabbitMQ based on the RPC pattern
    /// </summary>
    public interface IRabbitMqResponseService : IDisposable
    {
        /// <summary>
        /// Start listening to the selected queue and send responses
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="handleRequest"></param>
        public void StartListeningForRequests<TRequest, TResponse>(Func<TRequest, Task<TResponse>> handleRequest);
    }
}
