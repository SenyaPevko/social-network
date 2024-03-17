namespace Core.Logic.Connections.RabbitMqLogic.Geneartors.QueueNames
{
    /// <inheritdoc />
    public class QueueNameGenerator : IQueueNameGenerator
    {
        /// <inheritdoc />
        public string GenerateRequestQueueName<TModel>()
        {
            return $"Request{typeof(TModel)}";
        }

        /// <inheritdoc />
        public string GenerateResponseQueueName()
        {
            return $"Response{Guid.NewGuid()}";
        }
    }
}
