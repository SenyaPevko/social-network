namespace Core.RabbitMqLogic.Geneartors.Ids
{
    /// <summary>
    /// Id generator
    /// </summary>
    public interface IIdGenerator
    {
        /// <summary>
        /// Generates correlation id for RabbitMq requests
        /// </summary>
        /// <returns></returns>
        string GenerateCorrelationId();
    }
}
