namespace Core.RabbitMqLogic.Responses.Services
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
        /// <param name="handleRequest"></param>
        void StartListeningForRequests(Func<string, string> handleRequest);
    }
}
