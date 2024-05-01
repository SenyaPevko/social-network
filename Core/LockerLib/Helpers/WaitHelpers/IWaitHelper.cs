namespace LockerLib.Helpers.WaitHelpers;

/// <summary>
/// Provides methods for waiting cancellation.
/// </summary>
public interface IWaitHelper
{
    /// <summary>
    /// Asynchronously waits for cancellation.
    /// </summary>
    /// <param name="combinedToken">The combined cancellation token.</param>
    /// <param name="timeoutToken">The timeout cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, with a result indicating whether the cancellation was successful.</returns>

    Task<bool> WaitCancellationAsync(CancellationToken combinedToken, CancellationToken timeoutToken);
}