using System.Net.Mime;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Core.HttpLogic.HttpConnections.Services;
using Core.TraceLogic.TraceWriters;
using Core.HttpLogic.HttpConnections.Models;
using Core.HttpLogic.HttpResponses.Models;
using Core.HttpLogic.HttpRequests.Models;

namespace Core.HttpLogic.HttpRequests.Services
{
    /// <inheritdoc />
    internal class HttpRequestService : IHttpRequestService
    {
        private readonly IHttpConnectionService httpConnectionService;
        private readonly IEnumerable<ITraceWriter> traceWriterList;

        public HttpRequestService(
            IHttpConnectionService httpConnectionService,
            IEnumerable<ITraceWriter> traceWriterList)
        {
            this.httpConnectionService = httpConnectionService;
            this.traceWriterList = traceWriterList;
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

        private static HttpContent PrepairContent(object body, ContentType contentType)
        {
            switch (contentType)
            {
                case ContentType.ApplicationJson:
                    {
                        if (body is string stringBody)
                        {
                            body = JToken.Parse(stringBody);
                        }

                        var serializeSettings = new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            NullValueHandling = NullValueHandling.Ignore
                        };
                        var serializedBody = JsonConvert.SerializeObject(body, serializeSettings);
                        var content = new StringContent(serializedBody, Encoding.UTF8, MediaTypeNames.Application.Json);
                        return content;
                    }

                case ContentType.XWwwFormUrlEncoded:
                    {
                        if (body is not IEnumerable<KeyValuePair<string, string>> list)
                        {
                            throw new Exception(
                                $"Body for content type {contentType} must be {typeof(IEnumerable<KeyValuePair<string, string>>).Name}");
                        }

                        return new FormUrlEncodedContent(list);
                    }
                case ContentType.ApplicationXml:
                    {
                        if (body is not string s)
                        {
                            throw new Exception($"Body for content type {contentType} must be XML string");
                        }

                        return new StringContent(s, Encoding.UTF8, MediaTypeNames.Application.Xml);
                    }
                case ContentType.Binary:
                    {
                        if (body.GetType() != typeof(byte[]))
                        {
                            throw new Exception($"Body for content type {contentType} must be {typeof(byte[]).Name}");
                        }

                        return new ByteArrayContent((byte[])body);
                    }
                case ContentType.TextXml:
                    {
                        if (body is not string s)
                        {
                            throw new Exception($"Body for content type {contentType} must be XML string");
                        }

                        return new StringContent(s, Encoding.UTF8, MediaTypeNames.Text.Xml);
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(contentType), contentType, null);
            }
        }
    }
}
