namespace Core.HttpLogic.HttpConnections.Models;

public record struct HttpConnectionData()
{
    /// <summary>
    ///     Time that will be waited to get the response
    /// </summary>
    public TimeSpan? Timeout { get; set; } = null;

    /// <summary>
    ///     The token to request cancellation of task
    /// </summary>
    public CancellationToken CancellationToken { get; set; } = default;

    /// <summary>
    ///     The name of the client
    /// </summary>
    public string ClientName { get; set; }
}