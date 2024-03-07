using Microsoft.Extensions.Logging;
using Polly;

namespace Core.HttpLogic.Polly
{
    internal class HttpPolicy : IHttpPolicy
    {
        private readonly ILogger logger;
        public HttpPolicy(ILogger logger) 
        {
            this.logger = logger;
        }

        public IAsyncPolicy GetRetryPolicy(TimeSpan retryInterval, int retryCount = 3)
        {
            return Policy
                .Handle<HttpRequestException>()
                .Or<TaskCanceledException>()
                .WaitAndRetryAsync(retryCount, retryAttempt => retryInterval,
                (exception, timeSpan, retryCount, context) =>
                {   
                    logger.LogWarning($"Retry {retryCount} due to {exception}");
                });
        }

        public IAsyncPolicy GetTimeoutPolicy(TimeSpan timeout)
        {
            return Policy.TimeoutAsync(timeout);
        }
    }
}