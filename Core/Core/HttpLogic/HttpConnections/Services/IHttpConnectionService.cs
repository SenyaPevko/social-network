using Core.HttpLogic.HttpConnections.Models;

namespace Core.HttpLogic.HttpConnections.Services
{
    /// <summary>
    /// Http connection functionality
    /// </summary>
    internal interface IHttpConnectionService
    {
        /// <summary>
        /// Creating client for http connection
        /// </summary>
        /// <exception cref="HttpConnectionException"></exception>
        HttpClient CreateHttpClient(HttpConnectionData httpConnectionData);

        /// <summary>
        /// Send http request
        /// </summary>
        /// <exception cref="HttpConnectionException"></exception>
        Task<HttpResponseMessage> SendRequestAsync(
            HttpRequestMessage httpRequestMessage,
            HttpClient httpClient,
            CancellationToken cancellationToken,
            HttpCompletionOption httpCompletionOption = HttpCompletionOption.ResponseContentRead);
    }
}
