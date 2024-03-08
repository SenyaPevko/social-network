using System.Net;

namespace IdentityConnectionLib.ResponseHandlers
{
    public class ResponseHandler : IResponseHandler
    {
        public void HandleErrorResponse(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                default:
                    throw new HttpRequestException($"Request failed with status code: {statusCode}");
            }
        }
    }
}
