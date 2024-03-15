using RabbitMQ.Client;

namespace Core.Logic.Connections.RabbitMqLogic.Connections.Services
{
    /// <summary>
    /// RabbitMq connection functionality
    /// </summary>
    public interface IRabbitMqConnectionService: IDisposable
    {
        /// <summary>
        /// Creates channel connected to the RabbitMq server
        /// </summary>
        /// <returns></returns>
        IModel CreateChannel();

        /// <summary>
        /// Publishes a message to the server
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="message"></param>
        /// <param name="props"></param>
        /// <param name="exchangeName"></param>
        /// <param name="routingKey"></param>
        /// <param name="channel"></param>
        void PublishMessage<TMessage>(
            TMessage message,
            IBasicProperties props,
            string exchangeName,
            string routingKey,
            IModel channel);
    }
}
