namespace LockerLib.Models;

/// <summary>
/// Represents data associated with a lock.
/// </summary>
public class LockData
{
    private int lockCount;

    /// <summary>
    /// Initializes a new instance of the <see cref="LockData"/> class with the specified lock path.
    /// </summary>
    /// <param name="lockPath">The path associated with the lock.</param>
    public LockData(string lockPath)
    {
        LockPath = lockPath;
        lockCount = 1;
    }

    /// <summary>
    /// Gets the path associated with the lock.
    /// </summary>
    public string LockPath { get; }

    /// <summary>
    /// Increments the lock count.
    /// </summary>
    /// <returns>The incremented lock count.</returns>
    public int CountIncrement()
    {
        return ++lockCount;
    }

    /// <summary>
    /// Decrements the lock count.
    /// </summary>
    /// <returns>The decremented lock count.</returns>
    public int CountDecrement()
    {
        return --lockCount;
    }
}