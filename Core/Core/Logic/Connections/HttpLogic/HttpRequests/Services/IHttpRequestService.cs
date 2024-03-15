using Core.Logic.Connections.HttpLogic.HttpConnections.Models;
using Core.Logic.Connections.HttpLogic.HttpRequests.Models;
using Core.Logic.Connections.HttpLogic.HttpResponses.Models;

namespace Core.Logic.Connections.HttpLogic.HttpRequests.Services;

/// <summary>
///     Sending http requests and handling responses
/// </summary>
public interface IHttpRequestService
{
    /// <summary>
    ///     Send http request
    /// </summary>
    Task<HttpResponse<TResponse>> SendRequestAsync<TResponse>(
        HttpRequestData requestData,
        HttpConnectionData connectionData = default);
}