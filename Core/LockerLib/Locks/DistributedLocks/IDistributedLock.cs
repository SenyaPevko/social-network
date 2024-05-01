namespace LockerLib.Locks.DistributedLocks;

/// <summary>
/// Interface for a distributed lock.
/// </summary>
public interface IDistributedLock : IDisposable
{
    /// <summary>
    /// Tries to acquire the lock asynchronously.
    /// </summary>
    /// <param name="timeout">The timeout for acquiring the lock.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if the lock was successfully acquired; otherwise, false.</returns>
    Task<bool> AcquireAsync(TimeSpan timeout = default, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Releases the lock asynchronously.
    /// </summary>
    /// <returns>True if the lock was successfully released; otherwise, false.</returns>
    Task<bool> ReleaseAsync();
}