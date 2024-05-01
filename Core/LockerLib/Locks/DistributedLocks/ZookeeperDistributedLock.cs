using LockerLib.Locks.LockManagers;
using LockerLib.Models;

namespace LockerLib.Locks.DistributedLocks;

/// <summary>
/// Represents a distributed lock implemented using Zookeeper.
/// </summary>
internal class ZookeeperDistributedLock : IDistributedLock
{
    private readonly ILockManager zookeeperLockManager;
    private LockData? lockData;

    /// <summary>
    /// Initializes a new instance of the <see cref="ZookeeperDistributedLock"/> class.
    /// </summary>
    /// <param name="zookeeperLockManager">The Zookeeper lock manager.</param>
    public ZookeeperDistributedLock(ILockManager zookeeperLockManager)
    {
        this.zookeeperLockManager = zookeeperLockManager;
    }

    /// <inheritdoc cref="IDistributedLock.ReleaseAsync"/>>
    public async Task<bool> ReleaseAsync()
    {
        if (lockData == null) return false;

        var newLockCount = lockData.CountDecrement();
        if (newLockCount > 0)
            return true;

        await zookeeperLockManager.ReleaseLockAsync(lockData.LockPath);
        lockData = null;

        return true;
    }

    /// <inheritdoc cref="IDistributedLock.AcquireAsync"/>>
    public async Task<bool> AcquireAsync(TimeSpan timeout, CancellationToken cancellationToken)
    {
        if (lockData != null)
        {
            lockData.CountIncrement();
            return true;
        }

        var lockPath = await zookeeperLockManager.TryAcquireLockAsync(timeout, cancellationToken);
        if (string.IsNullOrEmpty(lockPath)) return false;
        lockData = new LockData(lockPath);

        return true;
    }

    /// <inheritdoc cref="IDistributedLock.Dispose"/>>
    public void Dispose()
    {
        zookeeperLockManager.Dispose();
    }
}