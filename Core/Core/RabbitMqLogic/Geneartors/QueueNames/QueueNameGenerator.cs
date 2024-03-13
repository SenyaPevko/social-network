namespace Core.RabbitMqLogic.Geneartors.QueueNames
{
    /// <inheritdoc />
    public class QueueNameGenerator : IQueueNameGenerator
    {
        /// <inheritdoc />
        public string GenerateRequestQueueName<TModel>()
        {
            return $"Request{nameof(TModel)}";
        }

        /// <inheritdoc />
        public string GenerateResponseQueueName()
        {
            return $"Response{Guid.NewGuid()}";
        }
    }
}
