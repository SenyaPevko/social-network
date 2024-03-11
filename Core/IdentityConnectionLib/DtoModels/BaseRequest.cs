namespace IdentityConnectionLib.DtoModels;

public record BaseRequest
{
    /// <summary>
    ///     Amount of retries that will be made in case if request won't be succesfull
    /// </summary>
    public int RetryCount { get; set; }

    /// <summary>
    ///     Interval in seconds that will be awaited before the next request retry call
    /// </summary>
    public TimeSpan RetryInterval { get; set; }

    /// <summary>
    ///     Interval in seconds that will be awaited before the next request retry call
    /// </summary>
    public TimeSpan ResponseAwaitTime { get; set; }
}