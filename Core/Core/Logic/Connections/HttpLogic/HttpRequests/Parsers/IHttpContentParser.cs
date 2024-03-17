namespace Core.Logic.Connections.HttpLogic.HttpRequests.Parsers
{
    /// <summary>
    ///     Parser from content type to HttpContent and from HttpContent to certain types
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface IHttpContentParser<T>
    {
        /// <summary>
        ///     Parses object of of a certain content type to HttpContent
        /// </summary>
        /// <param name="body"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public HttpContent ParseToHttpContent(object body, T contentType);

        /// <summary>
        ///     Parses HttpContent of certain contentType to object of chosen type
        /// </summary>
        /// <typeparam name="TContent"></typeparam>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public Task<TContent> ParseFromHttpContent<TContent>(HttpContent content, T contentType);
    }
}