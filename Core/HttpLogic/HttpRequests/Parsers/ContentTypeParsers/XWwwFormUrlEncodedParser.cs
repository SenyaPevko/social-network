﻿
using System.Net.Mime;

namespace Core.HttpLogic.HttpRequests.Parsers.ContentTypeParsers
{
    internal class XWwwFormUrlEncodedParser : IContentTypeParser
    {
        public ContentType SupportedContentType => ContentType.XWwwFormUrlEncoded;

        public HttpContent Parse(object body)
        {
            if (body is not IEnumerable<KeyValuePair<string, string>> list)
            {
                throw new Exception(
                    $"Body for content type {SupportedContentType} must be {typeof(IEnumerable<KeyValuePair<string, string>>).Name}");
            }

            return new FormUrlEncodedContent(list);
        }
    }
}
