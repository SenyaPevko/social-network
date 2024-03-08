using Core.HttpLogic.HttpConnections.Services;
using Core.TraceLogic.TraceWriters;
using Core.HttpLogic.HttpConnections.Models;
using Core.HttpLogic.HttpResponses.Models;
using Core.HttpLogic.HttpRequests.Models;
using Core.HttpLogic.HttpRequests.Parsers;
using Core.HttpLogic.Polly;
using Polly;

namespace Core.HttpLogic.HttpRequests.Services
{
    /// <inheritdoc />
    internal class HttpRequestService : IHttpRequestService
    {
        private readonly IHttpConnectionService httpConnectionService;
        private readonly IEnumerable<ITraceReader> traceReadersList;
        private readonly IHttpContentParser<ContentType> httpContentParser;
        private readonly IHttpPolicy httpPolicy;

        public HttpRequestService(
            IHttpConnectionService httpConnectionService,
            IEnumerable<ITraceReader> traceReadersList,
            IHttpContentParser<ContentType> httpContentParser,
            IHttpPolicy httpPolicy)
        {
            this.httpConnectionService = httpConnectionService;
            this.traceReadersList = traceReadersList;
            this.httpContentParser = httpContentParser;
            this.httpPolicy = httpPolicy;
        }

        /// <inheritdoc />
        public async Task<HttpResponse<TResponse>> SendRequestAsync<TResponse>(
            HttpRequestData requestData,
            HttpConnectionData connectionData = default)
        {
            ArgumentNullException.ThrowIfNull(requestData);

            var client = httpConnectionService.CreateHttpClient(connectionData);
            var content = httpContentParser.ParseToHttpContent(requestData.Body, requestData.ContentType);

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = requestData.Method,
                RequestUri = requestData.Uri, 
                Content = content
            };
            foreach (var traceReader in traceReadersList)
            {
                httpRequestMessage.Headers.Add(traceReader.Name, traceReader.GetValue());
            }
            var retryPolicy = httpPolicy.GetRetryPolicy(requestData.RetryInterval, requestData.RetryCount);
            var timeoutPolicy = httpPolicy.GetTimeoutPolicy(requestData.ResponseAwaitTime);
            var policyWrap = Policy.WrapAsync(retryPolicy, timeoutPolicy);
            var response = await policyWrap.ExecuteAsync(() => httpConnectionService
                .SendRequestAsync(httpRequestMessage, client, default));
            var responseContent = await httpContentParser.ParseFromHttpContent<TResponse>(response.Content, requestData.ContentType);
            
            return new HttpResponse<TResponse>()
            {
                Body = responseContent,
                StatusCode = response.StatusCode,
                Headers = response.Headers,
                ContentHeaders = response.Content.Headers,
            };
        }
    }
}
