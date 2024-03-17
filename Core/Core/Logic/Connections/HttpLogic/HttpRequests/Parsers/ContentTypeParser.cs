using Core.Logic.Connections.HttpLogic.HttpRequests.Parsers.ContentTypeParsers;

namespace Core.Logic.Connections.HttpLogic.HttpRequests.Parsers
{
    /// <inheritdoc />
    internal class ContentTypeParser : IHttpContentParser<ContentType>
    {
        private readonly IEnumerable<IContentTypeParser> parsers;

        public ContentTypeParser(IEnumerable<IContentTypeParser> parsers)
        {
            this.parsers = parsers;
        }

        /// <inheritdoc />
        public HttpContent ParseToHttpContent(object body, ContentType contentType)
        {
            var parser = parsers.First(parser => parser.SupportedContentType == contentType)
                         ?? throw new ArgumentOutOfRangeException(nameof(contentType), contentType, null);

            return parser.Parse(body);
        }

        /// <inheritdoc />
        public async Task<TContent> ParseFromHttpContent<TContent>(HttpContent content, ContentType contentType)
        {
            var parser = parsers.First(parser => parser.SupportedContentType == contentType)
                         ?? throw new ArgumentOutOfRangeException(nameof(contentType), contentType, null);

            return await parser.Parse<TContent>(content);
        }
    }
}