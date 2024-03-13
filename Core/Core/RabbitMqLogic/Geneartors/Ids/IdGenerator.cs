namespace Core.RabbitMqLogic.Geneartors.Ids
{
    /// <inheritdoc />
    public class IdGenerator : IIdGenerator
    {
        /// <inheritdoc />
        public string GenerateCorrelationId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
