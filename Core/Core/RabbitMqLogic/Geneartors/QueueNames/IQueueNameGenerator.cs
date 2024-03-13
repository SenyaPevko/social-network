namespace Core.RabbitMqLogic.Geneartors.QueueNames
{
    /// <summary>
    /// Names generator for RabbitMq queues
    /// </summary>
    public interface IQueueNameGenerator
    {
        /// <summary>
        /// Generate name for request queue
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        string GenerateRequestQueueName<TModel>();

        /// <summary>
        /// Generate name for response queue
        /// </summary>
        /// <returns></returns>
        string GenerateResponseQueueName();
    }
}
