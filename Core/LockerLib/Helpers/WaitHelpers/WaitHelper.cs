namespace LockerLib.Helpers.WaitHelpers;

/// <inheritdoc cref="IWaitHelper"/>>
public class WaitHelper : IWaitHelper
{
    /// <inheritdoc cref="IWaitHelper.WaitCancellationAsync"/>>
    public async Task<bool> WaitCancellationAsync(CancellationToken combinedToken, CancellationToken timeoutToken)
    {
        var waitCompletionSource = new TaskCompletionSource<bool>();

        using var timeoutRegistration =
            timeoutToken.Register(state => ((TaskCompletionSource<bool>)state).TrySetResult(false),
                waitCompletionSource);
        using var cancellationRegistration =
            combinedToken.Register(state => ((TaskCompletionSource<bool>)state).TrySetCanceled(), waitCompletionSource);

        try
        {
            return await waitCompletionSource.Task.ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            return false;
        }
    }
}