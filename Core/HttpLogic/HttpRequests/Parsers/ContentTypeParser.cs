using Core.HttpLogic.HttpRequests.Parsers.ContentTypeParsers;

namespace Core.HttpLogic.HttpRequests.Parsers
{
    internal class ContentTypeParser : IHttpContentParser<ContentType>
    {
        private readonly IEnumerable<IContentTypeParser> parsers;

        public ContentTypeParser(IEnumerable<IContentTypeParser> parsers)
        {
            this.parsers = parsers;
        }

        public HttpContent ParseToHttpContent(object body, ContentType contentType)
        {
            var parser = parsers.First(parser => parser.SupportedContentType == contentType)
                ?? throw new ArgumentOutOfRangeException(nameof(contentType), contentType, null);

            return parser.Parse(body);
        }
        public async Task<TContent> ParseFromHttpContent<TContent>(HttpContent content, ContentType contentType)
        {
            var parser = parsers.First(parser => parser.SupportedContentType == contentType)
                ?? throw new ArgumentOutOfRangeException(nameof(contentType), contentType, null);

            return await parser.Parse<TContent>(content);
        }
    }
}
