using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Core.HttpLogic.HttpRequests.Parsers.ContentTypeParsers
{
    internal class ApplicationXmlParser : IContentTypeParser
    {
        public ContentType SupportedContentType => ContentType.ApplicationXml;

        public HttpContent Parse(object body)
        {
            if (body is not string s)
            {
                throw new Exception($"Body for content type {SupportedContentType} must be XML string");
            }

            return new StringContent(s, Encoding.UTF8, MediaTypeNames.Application.Xml);
        }
    }
}
