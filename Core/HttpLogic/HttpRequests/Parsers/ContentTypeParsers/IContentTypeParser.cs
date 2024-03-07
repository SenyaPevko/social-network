﻿namespace Core.HttpLogic.HttpRequests.Parsers.ContentTypeParsers
{
    internal interface IContentTypeParser
    {
        public ContentType SupportedContentType { get; }
        public HttpContent Parse(object body);
    }
}
