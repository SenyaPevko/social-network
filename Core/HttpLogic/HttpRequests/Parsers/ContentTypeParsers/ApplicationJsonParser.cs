
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;

namespace Core.HttpLogic.HttpRequests.Parsers.ContentTypeParsers
{
    internal class ApplicationJsonParser : IContentTypeParser
    {
        public ContentType SupportedContentType => ContentType.ApplicationJson;

        public HttpContent Parse(object body)
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
    }
}
