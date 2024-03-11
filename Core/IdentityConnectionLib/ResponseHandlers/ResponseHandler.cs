using System.Net;

namespace IdentityConnectionLib.ResponseHandlers;

/// <inheritdoc />
public class ResponseHandler : IResponseHandler
{
    /// <inheritdoc />
    public void HandleErrorResponse(HttpStatusCode statusCode)
    {
        switch (statusCode)
        {
            default:
                throw new HttpRequestException($"Request failed with status code: {statusCode}");
        }
    }
}