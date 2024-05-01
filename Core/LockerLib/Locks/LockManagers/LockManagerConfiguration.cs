namespace LockerLib.Locks.LockManagers;

/// <summary>
/// Configuration for the lock manager.
/// </summary>
public class LockManagerConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LockManagerConfiguration"/> class with the specified maximum count.
    /// </summary>
    /// <param name="maxCount">The maximum count of locks.</param>
    public LockManagerConfiguration(int maxCount)
    {
        MaxCount = maxCount;
    }
    
    /// <summary>
    /// Gets the maximum count of locks.
    /// </summary>
    public int MaxCount { get; }
}