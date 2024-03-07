using Polly;

namespace Core.HttpLogic.Polly
{
    internal interface IHttpPolicy
    {
        IAsyncPolicy GetRetryPolicy(TimeSpan retryInterval, int retryCount = 3);

        IAsyncPolicy GetTimeoutPolicy(TimeSpan timeout);
    }
}
