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
        private readonly IToHttpContentParser<ContentType> contentTypeParser;

        public HttpRequestService(
            IHttpConnectionService httpConnectionService,
            IEnumerable<ITraceWriter> traceWriterList,
            IToHttpContentParser<ContentType> contentTypeParser)
        {
            this.httpConnectionService = httpConnectionService;
            this.traceWriterList = traceWriterList;
            this.contentTypeParser = contentTypeParser;
        }

        /// <inheritdoc />
        public async Task<HttpResponse<TResponse>> SendRequestAsync<TResponse>(
            HttpRequestData requestData,
            HttpConnectionData connectionData = default)
        {
            var client = httpConnectionService.CreateHttpClient(connectionData);

            // это обогащение запроса некимим заголовками для последующего востановления с помощью trace id
            var httpRequestMessage = new HttpRequestMessage();
            // тут происходит сборка trace'ов и добавление их в меседж котоорой пойдет в запрос
            foreach (var traceWriter in traceWriterList)
            {
                httpRequestMessage.Headers.Add(traceWriter.Name, traceWriter.GetValue());
            }
            var res = await httpConnectionService.SendRequestAsync(httpRequestMessage, client, default);
            return null;
        }
    }
}
