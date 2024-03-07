
using System.Net.Http;
using System.Net.Mime;

namespace Core.HttpLogic.HttpRequests.Parsers.ContentTypeParsers
{
    internal class BinaryParser : IContentTypeParser
    {
        public ContentType SupportedContentType => ContentType.Binary;

        public HttpContent Parse(object body)
        {
            if (body.GetType() != typeof(byte[]))
            {
                throw new Exception($"Body for content type {SupportedContentType} must be {typeof(byte[]).Name}");
            }

            return new ByteArrayContent((byte[])body);
        }

        public async Task<T> Parse<T>(HttpContent content)
        {
            var body = await content.ReadAsByteArrayAsync();
            var desirializedBody = (T)(object)body;

            return desirializedBody;
        }
    }
}
