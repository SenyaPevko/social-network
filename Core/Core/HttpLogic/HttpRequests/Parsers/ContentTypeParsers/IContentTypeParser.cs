namespace Core.HttpLogic.HttpRequests.Parsers.ContentTypeParsers;

/// <summary>
///     Parser from content type to HttpContent and back
/// </summary>
internal interface IContentTypeParser
{
    /// <summary>
    ///     Content type that the parser can parse
    /// </summary>
    public ContentType SupportedContentType { get; }

    /// <summary>
    ///     Parses from object to HttpContent
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    public HttpContent Parse(object body);

    /// <summary>
    ///     Parses from HttpContent to object of T type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="content"></param>
    /// <returns></returns>
    public Task<T> Parse<T>(HttpContent content);
}