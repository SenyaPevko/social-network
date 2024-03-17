using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Logic.Connections.HttpLogic.HttpRequests.Parsers.ContentTypeParsers
{
    /// <inheritdoc />
    internal class ApplicationJsonParser : IContentTypeParser
    {
        public ContentType SupportedContentType => ContentType.ApplicationJson;

        /// <inheritdoc />
        public HttpContent Parse(object body)
        {
            if (body is string stringBody) body = JToken.Parse(stringBody);

            var serializeSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };
            var serializedBody = JsonConvert.SerializeObject(body, serializeSettings);
            var content = new StringContent(serializedBody, Encoding.UTF8, MediaTypeNames.Application.Json);
            return content;
        }

        /// <inheritdoc />
        public async Task<T> Parse<T>(HttpContent content)
        {
            var body = await content.ReadAsStringAsync();
            var desirializedBody = JsonConvert.DeserializeObject<T>(body);

            return desirializedBody;
        }
    }
}