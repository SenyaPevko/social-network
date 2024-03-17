using System.Net;

namespace IdentityConnectionLib.ResponseHandlers;

/// <summary>
///     Handles http responses
/// </summary>
public interface IResponseHandler
{
    /// <summary>
    ///     Handles bad http responses that was not successful
    /// </summary>
    /// <param name="statusCode"></param>
    void HandleErrorResponse(HttpStatusCode statusCode);
}