namespace Core.RabbitMqLogic.Requests.Services
{
    /// <summary>
    /// Service for clients to publish message into their queue and 
    /// read messages from response queue using RabbitMQ based on the RPC pattern
    /// </summary>
    public interface IRabbitMqRequestService : IDisposable
    {
        /// <summary>
        /// Sends request to the server and waits for a response
        /// </summary>
        /// <param name="message"></param>
        /// <param name="correlationId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TResponse> SendRequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken);
    }
}
