namespace LockerLib.Locks;

/// <summary>
/// Configuration for paths used by the Zookeeper locker.
/// </summary>
public class ZookeeperLockerPathConfiguration
{
    private const string LockParent = "locks";
    private const string LockBaseName = "lock-";
    private const string LeaseParent = "leases";
    private const string LeaseBaseName = "lease-";

    /// <summary>
    /// Initializes a new instance of the <see cref="ZookeeperLockerPathConfiguration"/> class.
    /// </summary>
    /// <param name="semaphorePath">The path to the semaphore.</param>
    public ZookeeperLockerPathConfiguration(string semaphorePath)
    {
        SemaphorePath = semaphorePath;
    }

    /// <summary>
    /// Gets the parent path for locks.
    /// </summary>
    public string GetLockParent => LockParent;
    
    /// <summary>
    /// Gets the parent path for leases.
    /// </summary>
    public string GetLeaseParent => LeaseParent;
    
    /// <summary>
    /// Gets the base name for leases.
    /// </summary>
    public string GetLeaseBaseName => LeaseBaseName;
    
    /// <summary>
    /// Gets the base name for locks.
    /// </summary>
    public string GetLockBaseName => LockBaseName;

    /// <summary>
    /// Gets or sets the semaphore path.
    /// </summary>
    public string SemaphorePath { get; private set; }
}