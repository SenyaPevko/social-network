namespace Core.HttpLogic.HttpRequests.Parsers
{
    internal interface IHttpContentParser<T>
    {
        public HttpContent ParseToHttpContent(object body, T contentType);
        public Task<TContent> ParseFromHttpContent<TContent>(HttpContent content, T contentType);
    }
}
