using org.apache.zookeeper;

namespace LockerLib.Watchers;

/// <summary>
/// Represents a watcher for ZooKeeper events.
/// </summary>
public class ZookeeperWatcher : Watcher
{
    private readonly EventWaitHandle eventWaitHandle;

    /// <summary>
    /// Initializes a new instance of the <see cref="ZookeeperWatcher"/> class with the specified event wait handle.
    /// </summary>
    /// <param name="eventWaitHandle">The event wait handle to signal.</param>
    public ZookeeperWatcher(EventWaitHandle eventWaitHandle)
    {
        this.eventWaitHandle = eventWaitHandle;
    }

    /// <summary>
    /// Processes the ZooKeeper event.
    /// </summary>
    /// <param name="event">The ZooKeeper event to process.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public override Task process(WatchedEvent @event)
    {
        var state = @event.getState();
        if (state is Event.KeeperState.ConnectedReadOnly or Event.KeeperState.SyncConnected) eventWaitHandle.Set();

        return Task.FromResult(1);
    }
}