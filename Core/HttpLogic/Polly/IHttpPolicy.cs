using Polly;

namespace Core.HttpLogic.Polly
{
    internal interface IHttpPolicy
    {
        IAsyncPolicy GetRetryPolicy(
            int retryCount = 3,
            int retryIntervalInSeconds = 5);

        IAsyncPolicy GetTimeoutPolicy(TimeSpan timeout);
    }
}
