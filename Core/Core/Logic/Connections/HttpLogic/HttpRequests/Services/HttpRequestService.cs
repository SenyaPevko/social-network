﻿using Core.Logic.Connections.HttpLogic.HttpConnections.Models;
using Core.Logic.Connections.HttpLogic.HttpConnections.Services;
using Core.Logic.Connections.HttpLogic.HttpRequests.Models;
using Core.Logic.Connections.HttpLogic.HttpRequests.Parsers;
using Core.Logic.Connections.HttpLogic.HttpResponses.Models;
using Core.Logic.Connections.HttpLogic.Polly;
using Core.Logic.Tracing.TraceLogic.TraceWriters;
using Polly;

namespace Core.Logic.Connections.HttpLogic.HttpRequests.Services
{
    /// <inheritdoc />
    internal class HttpRequestService : IHttpRequestService
    {
        private readonly IHttpConnectionService httpConnectionService;
        private readonly IHttpContentParser<ContentType> httpContentParser;
        private readonly IHttpPolicy httpPolicy;
        private readonly IEnumerable<ITraceReader> traceReadersList;

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

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = requestData.Method,
                RequestUri = requestData.Uri,
                Content = content
            };
            EnrichTheMessageWithTraceId(httpRequestMessage);
            var retryPolicy = httpPolicy.GetRetryPolicy(requestData.RetryInterval, requestData.RetryCount);
            var timeoutPolicy = httpPolicy.GetTimeoutPolicy(requestData.ResponseAwaitTime);
            var policyWrap = Policy.WrapAsync(retryPolicy, timeoutPolicy);
            var response = await policyWrap.ExecuteAsync(() => httpConnectionService
                .SendRequestAsync(httpRequestMessage, client, default));
            var responseContent =
                await httpContentParser.ParseFromHttpContent<TResponse>(response.Content, requestData.ContentType);

            return new HttpResponse<TResponse>
            {
                Body = responseContent,
                StatusCode = response.StatusCode,
                Headers = response.Headers,
                ContentHeaders = response.Content.Headers
            };
        }

        private void EnrichTheMessageWithTraceId(HttpRequestMessage message)
        {
            foreach (var traceReader in traceReadersList)
                message.Headers.Add(traceReader.Name, traceReader.GetValue());
        }
    }
}