using Core.Logic.Connections.HttpLogic.HttpRequests;
using Newtonsoft.Json;

namespace Core.Logic.Connections.HttpLogic.HttpRequests.Parsers.ContentTypeParsers;

/// <inheritdoc />
internal class XWwwFormUrlEncodedParser : IContentTypeParser
{
    public ContentType SupportedContentType => ContentType.XWwwFormUrlEncoded;

    /// <inheritdoc />
    public HttpContent Parse(object body)
    {
        if (body is not IEnumerable<KeyValuePair<string, string>> list)
            throw new Exception(
                $"Body for content type {SupportedContentType} must be {typeof(IEnumerable<KeyValuePair<string, string>>).Name}");

        return new FormUrlEncodedContent(list);
    }

    /// <inheritdoc />
    public async Task<T> Parse<T>(HttpContent content)
    {
        var body = await content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<T>(body);
    }
}