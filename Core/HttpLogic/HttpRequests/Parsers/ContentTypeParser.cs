using Core.HttpLogic.HttpRequests.Parsers.ContentTypeParsers;

namespace Core.HttpLogic.HttpRequests.Parsers
{
    internal class ContentTypeParser : IToHttpContentParser<ContentType>
    {
        private readonly IEnumerable<IContentTypeParser> parsers;

        public ContentTypeParser(IEnumerable<IContentTypeParser> parsers)
        {
            this.parsers = parsers;
        }

        public HttpContent Parse(object body, ContentType contentType)
        {
            var parser = parsers.First(parser => parser.SupportedContentType == contentType)
                ?? throw new ArgumentOutOfRangeException(nameof(contentType), contentType, null);

            return parser.Parse(body);
        }
    }
}
