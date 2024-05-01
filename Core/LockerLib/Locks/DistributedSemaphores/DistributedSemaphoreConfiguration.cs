namespace LockerLib.Locks.DistributedSemaphores;

/// <summary>
/// Configuration settings for a distributed semaphore.
/// </summary>
public class DistributedSemaphoreConfiguration
{
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DistributedSemaphoreConfiguration"/> class.
    /// </summary>
    /// <param name="maxCount">The maximum count of the semaphore.</param>
    /// <param name="name">The optional name of the semaphore.</param>
    public DistributedSemaphoreConfiguration(int maxCount, string? name = null)
    {
        MaxCount = maxCount;
        Name = name;
    }

    /// <summary>
    /// Gets the maximum count of the semaphore.
    /// </summary>
    public int MaxCount { get; }
    
    /// <summary>
    /// Gets the optional name of the semaphore.
    /// </summary>
    public string? Name { get; }
}