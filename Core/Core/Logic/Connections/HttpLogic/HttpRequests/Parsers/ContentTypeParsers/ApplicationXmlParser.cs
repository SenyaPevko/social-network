using System.Net.Mime;
using System.Text;
using System.Xml.Serialization;
using Core.Logic.Connections.HttpLogic.HttpRequests;

namespace Core.Logic.Connections.HttpLogic.HttpRequests.Parsers.ContentTypeParsers;

/// <inheritdoc />
internal class ApplicationXmlParser : IContentTypeParser
{
    public ContentType SupportedContentType => ContentType.ApplicationXml;

    /// <inheritdoc />
    public HttpContent Parse(object body)
    {
        if (body is not string s)
            throw new Exception($"Body for content type {SupportedContentType} must be XML string");

        return new StringContent(s, Encoding.UTF8, MediaTypeNames.Application.Xml);
    }

    /// <inheritdoc />
    public async Task<T> Parse<T>(HttpContent content)
    {
        var serializer = new XmlSerializer(typeof(T));
        var desirializedBody = (T)serializer.Deserialize(await content.ReadAsStreamAsync());

        return desirializedBody;
    }
}