using System.Text;
using LockerLib.Helpers.PathHelpers;
using LockerLib.Watchers;
using Microsoft.Extensions.Options;
using org.apache.zookeeper;
using org.apache.zookeeper.data;
using static System.String;

namespace LockerLib.Clients;

/// <summary>
/// Represents the options for configuring the ZooKeeper client.
/// </summary>
public class ZooKeeperClient : ILockerClient
{
    private readonly IPathHelper zookeeperPathHelper;
    private AutoResetEvent autoResetEvent;
    private ZooKeeper zooKeeper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ZooKeeperClient"/> class.
    /// </summary>
    /// <param name="options">The options to configure the ZooKeeper client.</param>
    /// <param name="zookeeperPathHelper">The path helper instance.</param>
    public ZooKeeperClient(IOptions<ZookeeperClientOptions> options, IPathHelper zookeeperPathHelper)
    {
        ValidateTimeOut(options.Value.SessionTimeout.Milliseconds);
        Connect(options.Value.ConnectionString, options.Value.SessionTimeout);
        this.zookeeperPathHelper = zookeeperPathHelper;
    }

    /// <inheritdoc cref="ILockerClient.CreateLockNodeAsync"/>>
    public async Task<string> CreateLockNodeAsync(string path, byte[]? data)
    {
        return await zooKeeper.createAsync(path, data,
            ZooDefs.Ids.OPEN_ACL_UNSAFE, CreateMode.EPHEMERAL_SEQUENTIAL);
    }

    /// <inheritdoc cref="ILockerClient.IsNodeLocked"/>>
    public bool IsNodeLocked(List<string> sortedChildren, string sequenceNodePath, int maxLeases)
    {
        var sequenceNodeName = zookeeperPathHelper.GetNodeFromPath(sequenceNodePath);
        var nodeIndex = sortedChildren.IndexOf(sequenceNodeName);
        if (nodeIndex < 0)
            throw new KeeperException.NoNodeException(sequenceNodePath);

        return nodeIndex < maxLeases;
    }

    /// <inheritdoc cref="ILockerClient.GetSortedChildrenAsync"/>>
    public async Task<List<string>> GetSortedChildrenAsync(string basePath, string lockName)
    {
        var children = await zooKeeper.getChildrenAsync(basePath);

        var sortingResults = new Dictionary<string, string>();
        foreach (var child in children.Children)
            sortingResults[child] = zookeeperPathHelper.ExtractSuffixForNodesSorting(child, lockName);

        children.Children.Sort((node1, node2) =>
            Compare(sortingResults[node1], sortingResults[node2], StringComparison.Ordinal));

        return children.Children;
    }

    /// <inheritdoc cref="ILockerClient.SetWatcherOnNodeAsync"/>>
    public async Task SetWatcherOnNodeAsync(string nodePath, Watcher watcher)
    {
        await zooKeeper.getDataAsync(nodePath, watcher);
    }

    /// <inheritdoc cref="ILockerClient.GetChildrenAsync"/>>
    public async Task<ChildrenResult> GetChildrenAsync(string path, Watcher watcher)
    {
        return await zooKeeper.getChildrenAsync(path, watcher);
    }

    /// <inheritdoc cref="ILockerClient.DeleteNodeAsync"/>>
    public async Task DeleteNodeAsync(string nodePath)
    {
        try
        {
            await zooKeeper.deleteAsync(nodePath);
        }
        catch (KeeperException.NoNodeException e)
        {
        }
    }

    /// <inheritdoc cref="ILockerClient.GetNodeDataAsync"/>>
    public async Task<int> GetNodeDataAsync(string path)
    {
        var dataResult = await zooKeeper.getDataAsync(path);

        if (dataResult?.Data == null)
            throw new InvalidOperationException("No data was found for the specified path: " + path);

        var data = Encoding.UTF8.GetString(dataResult.Data);
        if (!int.TryParse(data, out var result))
            throw new FormatException("Failed to convert the data to an integer for the path: " + path);

        return result;
    }

    /// <inheritdoc cref="ILockerClient.CreateLeaseNodeAsync"/>
    public async Task<string> CreateLeaseNodeAsync(string path, byte[] data)
    {
        return await CreateNodeRecursivelyAsync(path, data, ZooDefs.Ids.OPEN_ACL_UNSAFE,
            CreateMode.PERSISTENT);
    }

    /// <inheritdoc cref="ILockerClient.IsConnected"/>
    public bool IsConnected()
    {
        return zooKeeper.getState() == ZooKeeper.States.CONNECTED;
    }

    /// <inheritdoc cref="ILockerClient.Dispose"/>
    public void Dispose()
    {
        zooKeeper.closeAsync().Wait();
        autoResetEvent.Dispose();
        GC.Collect();
    }

    private static void ValidateTimeOut(int timeout)
    {
        switch (timeout)
        {
            case 0:
                throw new ArgumentOutOfRangeException();
            case Timeout.Infinite:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Connect(string connectionString, TimeSpan sessionTimeout)
    {
        autoResetEvent = new AutoResetEvent(false);
        var watcher = new ZookeeperWatcher(autoResetEvent);
        autoResetEvent.Reset();
        zooKeeper = new ZooKeeper(connectionString, sessionTimeout.Milliseconds, watcher);
        if (!autoResetEvent.WaitOne(sessionTimeout)) throw new TimeoutException();
    }

    private async Task<string> CreateNodeRecursivelyAsync(string path, byte[] data, List<ACL> acls,
        CreateMode createMode)
    {
        path = zookeeperPathHelper.ValidatePath(path);
        var paths = zookeeperPathHelper.SplitPath(path);
        var nodePath = path;
        var currentPath = Empty;

        foreach (var segment in paths)
        {
            currentPath += zookeeperPathHelper.GetPathSeparator + segment;

            try
            {
                var stat = await zooKeeper.existsAsync(currentPath);
                if (stat == null)
                {
                    if (segment == paths.Last())
                        nodePath = await zooKeeper.createAsync(currentPath, data, acls, createMode);
                    else
                        await zooKeeper.createAsync(currentPath, null, acls, createMode);
                }
            }
            catch (KeeperException.NodeExistsException)
            {
            }
        }

        return nodePath;
    }
}