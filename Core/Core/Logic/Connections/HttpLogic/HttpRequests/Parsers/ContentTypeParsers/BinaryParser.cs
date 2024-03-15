using Core.Logic.Connections.HttpLogic.HttpRequests;

namespace Core.Logic.Connections.HttpLogic.HttpRequests.Parsers.ContentTypeParsers;

/// <inheritdoc />
internal class BinaryParser : IContentTypeParser
{
    public ContentType SupportedContentType => ContentType.Binary;

    /// <inheritdoc />
    public HttpContent Parse(object body)
    {
        if (body.GetType() != typeof(byte[]))
            throw new Exception($"Body for content type {SupportedContentType} must be {typeof(byte[]).Name}");

        return new ByteArrayContent((byte[])body);
    }

    /// <inheritdoc />
    public async Task<T> Parse<T>(HttpContent content)
    {
        var body = await content.ReadAsByteArrayAsync();
        var desirializedBody = (T)(object)body;

        return desirializedBody;
    }
}