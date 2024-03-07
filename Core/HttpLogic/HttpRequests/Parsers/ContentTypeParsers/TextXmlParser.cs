using System.Net.Mime;
using System.Text;

namespace Core.HttpLogic.HttpRequests.Parsers.ContentTypeParsers
{
    internal class TextXmlParser : IContentTypeParser
    {
        public ContentType SupportedContentType => ContentType.TextXml;

        public HttpContent Parse(object body)
        {
            if (body is not string s)
            {
                throw new Exception($"Body for content type {SupportedContentType} must be XML string");
            }

            return new StringContent(s, Encoding.UTF8, MediaTypeNames.Text.Xml);
        }
    }
}
