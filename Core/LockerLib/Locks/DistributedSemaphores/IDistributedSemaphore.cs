namespace LockerLib.Locks.DistributedSemaphores;

/// <summary>
/// Represents a distributed semaphore.
/// </summary>
public interface IDistributedSemaphore
{
    /// <summary>
    /// Gets the maximum count of the semaphore.
    /// </summary>
    int MaxCount { get; }
    
    /// <summary>
    /// Gets the name of the semaphore.
    /// </summary>
    string? Name { get; }
    
    /// <summary>
    /// Gets the current count of the semaphore.
    /// </summary>
    int CurrentCount { get; }
    
    /// <summary>
    /// Asynchronously acquires a lock within the specified timeout.
    /// </summary>
    /// <param name="timeout">The timeout for acquiring the semaphore.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if a lock was successfully acquired within the timeout; otherwise, false.</returns>
    Task<bool> AcquireAsync(TimeSpan timeout, CancellationToken cancellationToken);
    
    /// <summary>
    /// Asynchronously releases a lock.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ReleaseAsync();
}