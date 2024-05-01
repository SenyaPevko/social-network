namespace LockerLib;

/// <summary>
/// Represents options for configuring the Zookeeper lock.
/// </summary>
public class ZookeeperLockOptions
{
    
    /// <summary>
    /// Gets or sets the connection string for Zookeeper.
    /// </summary>
    public required string ConnectionString { get; set; }
    
    /// <summary>
    /// Gets or sets the session timeout for Zookeeper.
    /// </summary>
    public required TimeSpan SessionTimeout { get; set; }
    
    /// <summary>
    /// Gets or sets the path for the semaphore in Zookeeper.
    /// </summary>
    public required string SemaphorePath { get; set; }
    
    /// <summary>
    /// Gets or sets the maximum count for the semaphore.
    /// </summary>
    public required int SemaphoreMaxCount { get; set; }
    
    /// <summary>
    /// Gets or sets the name of the semaphore.
    /// </summary>
    public string? SemaphoreName { get; set; }
}