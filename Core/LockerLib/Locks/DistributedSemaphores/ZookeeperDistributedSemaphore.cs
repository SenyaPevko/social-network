using System.Text;
using LockerLib.Clients;
using LockerLib.Helpers.PathHelpers;
using LockerLib.Helpers.WaitHelpers;
using LockerLib.Locks.DistributedLocks;
using LockerLib.Watchers;
using org.apache.zookeeper;

namespace LockerLib.Locks.DistributedSemaphores;

/// <summary>
/// Represents a distributed semaphore implemented using ZooKeeper.
/// </summary>
public class ZookeeperDistributedSemaphore : IDistributedSemaphore
{
    private readonly List<string> acquiredLeases;
    private readonly IDistributedLock distributedLock;
    private readonly string leaseBaseName;
    private readonly string leasesPath;
    private readonly SemaphoreSlim semaphore;
    private readonly IWaitHelper waitHelper;
    private readonly Watcher watcher;
    private readonly ILockerClient zooKeeperClient;
    private readonly IPathHelper zookeeperPathHelper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ZookeeperDistributedSemaphore"/> class.
    /// </summary>
    /// <param name="zooKeeperClient">The ZooKeeper client.</param>
    /// <param name="distributedLock">The distributed lock manager.</param>
    /// <param name="zookeeperPathHelper">The ZooKeeper path helper.</param>
    /// <param name="waitHelper">The wait helper.</param>
    /// <param name="pathConfiguration">The ZooKeeper locker path configuration.</param>
    /// <param name="semaphoreConfiguration">The distributed semaphore configuration.</param>
    public ZookeeperDistributedSemaphore(
        ILockerClient zooKeeperClient,
        IDistributedLock distributedLock,
        IPathHelper zookeeperPathHelper,
        IWaitHelper waitHelper,
        ZookeeperLockerPathConfiguration pathConfiguration,
        DistributedSemaphoreConfiguration semaphoreConfiguration)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(semaphoreConfiguration.MaxCount, 1);
        MaxCount = semaphoreConfiguration.MaxCount;
        Name = semaphoreConfiguration.Name;

        leasesPath =
            zookeeperPathHelper.ConstructPath(pathConfiguration.SemaphorePath, pathConfiguration.GetLeaseParent);
        leaseBaseName = pathConfiguration.GetLeaseBaseName;

        this.zookeeperPathHelper = zookeeperPathHelper;
        this.waitHelper = waitHelper;
        this.zooKeeperClient = zooKeeperClient;
        this.distributedLock = distributedLock;

        acquiredLeases = new List<string>(MaxCount);
        semaphore = new SemaphoreSlim(0);
        watcher = new ReleaseLockWatcher(semaphore);
    }

    /// <summary>
    /// Gets the maximum count of the semaphore.
    /// </summary>
    public int MaxCount { get; }
    
    /// <summary>
    /// Gets the optional name of the semaphore.
    /// </summary>
    public string? Name { get; }
    
    /// <summary>
    /// Gets the current count of acquired leases.
    /// </summary>
    public int CurrentCount => acquiredLeases.Count;

    
    /// <inheritdoc cref="IDistributedSemaphore.AcquireAsync"/>>
    public async Task<bool> AcquireAsync(TimeSpan timeout, CancellationToken cancellationToken)
    {
        var count = await EnsureCorrectLeasesCountAsync(leasesPath, MaxCount);
        if (count < 1)
            throw new InvalidOperationException();

        var leasePath = await AcquireLeaseNodeAsync(timeout, cancellationToken);
        if (string.IsNullOrEmpty(leasePath))
            return false;
        acquiredLeases.Add(leasePath);

        return true;
    }

    /// <inheritdoc cref="IDistributedSemaphore.ReleaseAsync"/>>
    public async Task ReleaseAsync()
    {
        if (acquiredLeases.Count == 0) return;
        await zooKeeperClient.DeleteNodeAsync(acquiredLeases.First());
        acquiredLeases.RemoveAt(0);
    }

    private async Task<string?> AcquireLeaseNodeAsync(TimeSpan timeout, CancellationToken cancellationToken)
    {
        if (!await distributedLock.AcquireAsync(timeout, cancellationToken))
            return null;

        var leasePath = await zooKeeperClient
            .CreateLockNodeAsync(zookeeperPathHelper.ConstructPath(leasesPath, leaseBaseName), null);
        var leaseNodeName = zookeeperPathHelper.GetNodeFromPath(leasePath);

        if (!await WaitForLeaseNodeToBecomeAvailableAsync(leaseNodeName, timeout, cancellationToken))
        {
            await zooKeeperClient.DeleteNodeAsync(leasePath);
            await distributedLock.ReleaseAsync();
            return null;
        }

        await distributedLock.ReleaseAsync();

        return leasePath;
    }

    private async Task<bool> WaitForLeaseNodeToBecomeAvailableAsync(
        string leaseNodeName, TimeSpan timeout, CancellationToken cancellationToken)
    {
        var timeoutCancellationSource = new CancellationTokenSource(timeout);
        var combinedTokenSource =
            CancellationTokenSource.CreateLinkedTokenSource(timeoutCancellationSource.Token, cancellationToken);

        while (true)
        {
            var childrenResult = await zooKeeperClient.GetChildrenAsync(leasesPath, watcher);

            if (!childrenResult.Children.Contains(leaseNodeName)) return false;

            if (childrenResult.Children.Count <= MaxCount) return true;

            var waitCancellation = waitHelper.WaitCancellationAsync(
                combinedTokenSource.Token, timeoutCancellationSource.Token);
            var waitNodeRelease = WaitForNodeReleaseAsync(timeout, combinedTokenSource.Token);
            if (!await waitCancellation || !await waitNodeRelease) return false;
        }
    }

    private async Task<bool> WaitForNodeReleaseAsync(TimeSpan timeout, CancellationToken cancellationToken)
    {
        try
        {
            return await semaphore.WaitAsync(timeout, cancellationToken);
        }
        catch (KeeperException.NoNodeException e)
        {
            return true;
        }
    }

    private async Task<int> EnsureCorrectLeasesCountAsync(string leasesPath, int maxLeases)
    {
        try
        {
            var count = await zooKeeperClient.GetNodeDataAsync(leasesPath);
            if (count != maxLeases)
                throw new InvalidOperationException(
                    "The number of leases in the node does not match the expected value of maxLeases.");

            return count;
        }
        catch (KeeperException.NoNodeException)
        {
            var data = Encoding.UTF8.GetBytes(maxLeases.ToString());
            await zooKeeperClient.CreateLeaseNodeAsync(leasesPath, data);
        }

        return maxLeases;
    }

    public void Dispose()
    {
        semaphore.Dispose();
        distributedLock.Dispose();
    }
}