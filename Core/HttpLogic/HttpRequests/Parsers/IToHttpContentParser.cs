namespace Core.HttpLogic.HttpRequests.Parsers
{
    internal interface IToHttpContentParser<T>
    {
        public HttpContent Parse(object body, T contentType);
    }
}
