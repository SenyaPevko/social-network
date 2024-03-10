using Polly;

namespace Core.HttpLogic.Polly
{
    /// <summary>
    /// Policy for http protocol requests and responses
    /// </summary>
    internal interface IHttpPolicy
    {
        /// <summary>
        /// Get policy for retrying requests
        /// </summary>
        /// <param name="retryInterval"></param>
        /// <param name="retryCount"></param>
        /// <returns></returns>
        IAsyncPolicy GetRetryPolicy(TimeSpan retryInterval, int retryCount = 3);

        /// <summary>
        /// Get policy for waiting to response to come
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        IAsyncPolicy GetTimeoutPolicy(TimeSpan timeout);
    }
}
