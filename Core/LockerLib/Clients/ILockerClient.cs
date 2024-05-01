using org.apache.zookeeper;

namespace LockerLib.Clients;

/// <summary>
/// Provides functionality to interact with a ZooKeeper instance for locking purposes.
/// </summary>
public interface ILockerClient
{
    /// <summary>
    /// Creates a lock node asynchronously.
    /// </summary>
    /// <param name="path">The path of the lock node to create.</param>
    /// <param name="data">The data to set for the lock node.</param>
    /// <returns>The created lock node's path.</returns>
    Task<string> CreateLockNodeAsync(string path, byte[]? data);
    
    /// <summary>
    /// Checks if a node is locked based on the given list of sorted children and sequence node path.
    /// </summary>
    /// <param name="sortedChildren">The list of sorted children.</param>
    /// <param name="sequenceNodePath">The path of the sequence node to check.</param>
    /// <param name="maxLeases">The maximum number of leases allowed.</param>
    /// <returns><see langword="true"/> if the node is locked; otherwise, <see langword="false"/>.</returns>
    bool IsNodeLocked(List<string> sortedChildren, string sequenceNodePath, int maxLeases);

    /// <summary>
    /// Gets the sorted children of a node asynchronously.
    /// </summary>
    /// <param name="basePath">The base path of the node.</param>
    /// <param name="lockName">The name of the lock.</param>
    /// <returns>A list of sorted children.</returns>
    Task<List<string>> GetSortedChildrenAsync(string basePath, string lockName);

    /// <summary>
    /// Sets a watcher on a node asynchronously.
    /// </summary>
    /// <param name="nodePath">The path of the node to set the watcher on.</param>
    /// <param name="watcher">The watcher to set.</param>
    Task SetWatcherOnNodeAsync(string nodePath, Watcher watcher);

    /// <summary>
    /// Gets the children of a node asynchronously.
    /// </summary>
    /// <param name="path">The path of the node.</param>
    /// <param name="watcher">The watcher to set.</param>
    /// <returns>The children result.</returns>
    Task<ChildrenResult> GetChildrenAsync(string path, Watcher watcher);

    /// <summary>
    /// Deletes a node asynchronously.
    /// </summary>
    /// <param name="nodePath">The path of the node to delete.</param>
    Task DeleteNodeAsync(string nodePath);

    /// <summary>
    /// Gets the data of a node asynchronously.
    /// </summary>
    /// <param name="path">The path of the node.</param>
    /// <returns>The node's data.</returns>
    Task<int> GetNodeDataAsync(string path);

    /// <summary>
    /// Creates a lease node asynchronously.
    /// </summary>
    /// <param name="path">The path of the lease node to create.</param>
    /// <param name="data">The data to set for the lease node.</param>
    /// <returns>The created lease node's path.</returns>
    Task<string> CreateLeaseNodeAsync(string path, byte[] data);

    /// <summary>
    /// Checks if the client is connected to ZooKeeper.
    /// </summary>
    /// <returns><see langword="true"/> if connected; otherwise, <see langword="false"/>.</returns>
    bool IsConnected();

    /// <summary>
    /// Disposes the client.
    /// </summary>
    void Dispose();
}