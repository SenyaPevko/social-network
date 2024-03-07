using Core.HttpLogic.HttpConnections.Services;
using Core.TraceLogic.TraceWriters;
using Core.HttpLogic.HttpConnections.Models;
using Core.HttpLogic.HttpResponses.Models;
using Core.HttpLogic.HttpRequests.Models;
using Core.HttpLogic.HttpRequests.Parsers;

namespace Core.HttpLogic.HttpRequests.Services
{
    /// <inheritdoc />
    internal class HttpRequestService : IHttpRequestService
    {
        private readonly IHttpConnectionService httpConnectionService;
        private readonly IEnumerable<ITraceWriter> traceWriterList;
        private readonly IHttpContentParser<ContentType> httpContentParser;

        public HttpRequestService(
            IHttpConnectionService httpConnectionService,
            IEnumerable<ITraceWriter> traceWriterList,
            IHttpContentParser<ContentType> contentTypeParser)
        {
            this.httpConnectionService = httpConnectionService;
            this.traceWriterList = traceWriterList;
            this.httpContentParser = contentTypeParser;
        }

        /// <inheritdoc />
        public async Task<HttpResponse<TResponse>> SendRequestAsync<TResponse>(
            HttpRequestData requestData,
            HttpConnectionData connectionData = default)
        {
            ArgumentNullException.ThrowIfNull(requestData);

            var client = httpConnectionService.CreateHttpClient(connectionData);
            var content = httpContentParser.ParseToHttpContent(requestData.Body, requestData.ContentType);

            // это обогащение запроса некимим заголовками для последующего востановления с помощью trace id
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = requestData.Method,
                RequestUri = requestData.Uri, 
                Content = content
            };
            // тут происходит сборка trace'ов и добавление их в меседж котоорой пойдет в запрос
            foreach (var traceWriter in traceWriterList)
            {
                httpRequestMessage.Headers.Add(traceWriter.Name, traceWriter.GetValue());
            }
            var response = await httpConnectionService.SendRequestAsync(httpRequestMessage, client, default);
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
