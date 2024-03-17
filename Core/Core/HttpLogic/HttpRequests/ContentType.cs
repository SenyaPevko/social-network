namespace Core.HttpLogic.HttpRequests;

public enum ContentType
{
    /// <summary>
    ///     Unknown content type
    /// </summary>
    Unknown = 0,

    /// <summary>
    ///     ApplicationJson content type
    /// </summary>
    ApplicationJson = 1,

    /// <summary>
    ///     XWwwFormUrlEncoded content type
    /// </summary>
    XWwwFormUrlEncoded = 2,

    /// <summary>
    ///     Binary content type
    /// </summary>
    Binary = 3,

    /// <summary>
    ///     ApplicationXml content type
    /// </summary>
    ApplicationXml = 4,

    /// <summary>
    ///     MultipartFormData content type
    /// </summary>
    MultipartFormData = 5,

    /// <summary>
    ///     TextXml content type
    /// </summary>
    TextXml = 6,

    /// <summary>
    ///     TextPlain content type
    /// </summary>
    TextPlain = 7,

    /// <summary>
    ///     ApplicationJwt content type
    /// </summary>
    ApplicationJwt = 8
}