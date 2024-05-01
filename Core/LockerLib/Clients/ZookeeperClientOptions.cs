namespace LockerLib.Clients;

/// <summary>
/// Represents the options for configuring the ZooKeeper client.
/// </summary>
public record ZookeeperClientOptions
{
    /// <summary>
    /// Gets or sets the connection string to the ZooKeeper server.
    /// </summary>
    public required string ConnectionString { get; init; }
    
    /// <summary>
    /// Gets or sets the session timeout for the ZooKeeper client.
    /// </summary>
    public required TimeSpan SessionTimeout { get; init; }
}