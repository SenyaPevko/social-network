using org.apache.zookeeper;

namespace LockerLib.Watchers;

/// <summary>
/// Represents a watcher for releasing a lock.
/// </summary>
public class ReleaseLockWatcher : Watcher
{
    private readonly SemaphoreSlim semaphore;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReleaseLockWatcher"/> class with the specified semaphore.
    /// </summary>
    /// <param name="semaphore">The semaphore to release.</param>
    public ReleaseLockWatcher(SemaphoreSlim semaphore)
    {
        this.semaphore = semaphore;
    }
    
    /// <summary>
    /// Processes the ZooKeeper event.
    /// </summary>
    /// <param name="event">The ZooKeeper event to process.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public override Task process(WatchedEvent @event)
    {
        semaphore.Release();
        return Task.CompletedTask;
    }
}