using System.Net;

namespace IdentityConnectionLib.ResponseHandlers
{
    public interface IResponseHandler
    {
        void HandleErrorResponse(HttpStatusCode statusCode);
    }
}
