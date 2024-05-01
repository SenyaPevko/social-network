using LockerLib.Clients;
using LockerLib.Helpers.PathHelpers;
using LockerLib.Helpers.WaitHelpers;
using LockerLib.Locks.DistributedSemaphores;
using LockerLib.Watchers;
using org.apache.zookeeper;

namespace LockerLib.Locks.LockManagers;

/// <summary>
/// Manages locks using Zookeeper.
/// </summary>
internal class ZookeeperLockManager : ILockManager
{
    private readonly string basePath;
    private readonly string lockName;
    private readonly string lockPath;
    private readonly int maxCount;
    private readonly SemaphoreSlim semaphore;
    private readonly IWaitHelper waitHelper;
    private readonly Watcher watcher;
    private readonly ILockerClient zooKeeperClient;
    private readonly IPathHelper zookeeperPathHelper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ZookeeperLockManager"/> class.
    /// </summary>
    /// <param name="zooKeeperClient">The Zookeeper client.</param>
    /// <param name="zookeeperPathHelper">The Zookeeper path helper.</param>
    /// <param name="waitHelper">The wait helper.</param>
    /// <param name="lockManagerConfiguration">The lock manager configuration.</param>
    /// <param name="pathConfiguration">The path configuration.</param>
    public ZookeeperLockManager(
        ILockerClient zooKeeperClient,
        IPathHelper zookeeperPathHelper,
        IWaitHelper waitHelper,
        LockManagerConfiguration lockManagerConfiguration,
        ZookeeperLockerPathConfiguration pathConfiguration)
    {
        maxCount = lockManagerConfiguration.MaxCount;
        basePath = zookeeperPathHelper.ConstructPath(pathConfiguration.SemaphorePath,
            pathConfiguration.GetLockParent);
        lockName = pathConfiguration.GetLockBaseName;
        lockPath = zookeeperPathHelper.ConstructPath(basePath, lockName);
        semaphore = new SemaphoreSlim(0, maxCount);
        this.zooKeeperClient = zooKeeperClient;
        this.zookeeperPathHelper = zookeeperPathHelper;
        this.waitHelper = waitHelper;
        watcher = new ReleaseLockWatcher(semaphore);
    }

    /// <inheritdoc cref="ILockManager.TryAcquireLockAsync"/>>
    public async Task<string?> TryAcquireLockAsync(TimeSpan timeout, CancellationToken cancellationToken)
    {
        var nodePath = await zooKeeperClient.CreateLockNodeAsync(lockPath, null);
        var hasTheLock = await AcquireLockWithRetryAsync(nodePath, timeout, cancellationToken);

        return hasTheLock ? nodePath : null;
    }

    /// <inheritdoc cref="ILockManager.ReleaseLockAsync"/>>
    public async Task ReleaseLockAsync(string lockPath)
    {
        await zooKeeperClient.DeleteNodeAsync(lockPath);
    }

    /// <inheritdoc cref="ILockManager.Dispose"/>>
    public void Dispose()
    {
        semaphore.Dispose();
        zooKeeperClient.DeleteNodeAsync(basePath).GetAwaiter().GetResult();
    }

    private async Task<bool> AcquireLockWithRetryAsync(string nodePath, TimeSpan timeout,
        CancellationToken cancellationToken)
    {
        var timeoutCancellationSource = new CancellationTokenSource(timeout);
        var combinedTokenSource =
            CancellationTokenSource.CreateLinkedTokenSource(timeoutCancellationSource.Token, cancellationToken);

        while (zooKeeperClient.IsConnected())
        {
            var children = await zooKeeperClient.GetSortedChildrenAsync(basePath, lockName);
            if (zooKeeperClient.IsNodeLocked(children, nodePath, maxCount))
                return true;

            var nodeToWatch = children[children.IndexOf(nodePath) - maxCount];
            var previousSequencePath = zookeeperPathHelper.ConstructPath(basePath, nodeToWatch);

            var waitCancellation = waitHelper.WaitCancellationAsync(combinedTokenSource.Token,
                timeoutCancellationSource.Token);
            var waitNodeRelease = WaitForNodeReleaseAsync(previousSequencePath, timeout, combinedTokenSource.Token);
            if (!await waitCancellation || !await waitNodeRelease)
            {
                await zooKeeperClient.DeleteNodeAsync(nodePath);
                break;
            }
        }

        return false;
    }

    private async Task<bool> WaitForNodeReleaseAsync(string nodePath, TimeSpan timeout,
        CancellationToken cancellationToken)
    {
        try
        {
            await zooKeeperClient.SetWatcherOnNodeAsync(nodePath, watcher);

            return await semaphore.WaitAsync(timeout, cancellationToken);
        }
        catch (KeeperException.NoNodeException e)
        {
            return true;
        }
    }
}