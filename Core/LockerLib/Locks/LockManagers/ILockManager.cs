namespace LockerLib.Locks.LockManagers;

/// <summary>
/// Interface for managing locks.
/// </summary>
public interface ILockManager : IDisposable
{
    /// <summary>
    /// Tries to acquire a lock asynchronously.
    /// </summary>
    /// <param name="timeout">The timeout for acquiring the lock.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The path of the acquired lock, or null if the lock acquisition failed.</returns>
    Task<string?> TryAcquireLockAsync(TimeSpan timeout, CancellationToken cancellationToken);
    
    /// <summary>
    /// Releases the lock asynchronously.
    /// </summary>
    /// <param name="lockPath">The path of the lock to release.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ReleaseLockAsync(string lockPath);
}