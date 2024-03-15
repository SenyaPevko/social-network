namespace Core.Logic.Connections.RabbitMqLogic.Connections.Models
{
    /// <summary>
    /// RabbitMq connection type
    /// </summary>
    public enum RabbitMqConnectionType
    {
        /// <summary>
        /// Connection to RabbitMq by uri
        /// </summary>
        Uri = 0,

        /// <summary>
        /// Connection to RabbitMq by uri host name
        /// </summary>
        Host = 1,
    }
}
