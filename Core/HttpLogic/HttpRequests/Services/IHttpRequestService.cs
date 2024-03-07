﻿using Core.HttpLogic.HttpConnections.Models;
using Core.HttpLogic.HttpConnections.Services;
using Core.HttpLogic.HttpRequests.Models;
using Core.HttpLogic.HttpResponses.Models;

namespace Core.HttpLogic.HttpRequests.Services
{
    /// <summary>
    /// Sending http requests and handling responses
    /// </summary>
    internal interface IHttpRequestService
    {
        /// <summary>
        /// Send http request
        /// </summary>
        Task<HttpResponse<TResponse>> SendRequestAsync<TResponse>(
            HttpRequestData requestData,
            HttpConnectionData connectionData = default);
    }
}
