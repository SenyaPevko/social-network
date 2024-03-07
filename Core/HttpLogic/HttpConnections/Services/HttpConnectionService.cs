using Core.HttpLogic.HttpConnections.Models;

namespace Core.HttpLogic.HttpConnections.Services
{
    /// <inheritdoc />
    internal class HttpConnectionService : IHttpConnectionService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public HttpConnectionService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        /// <inheritdoc />
        public HttpClient CreateHttpClient(HttpConnectionData httpConnectionData)
        {
            var httpClient = string.IsNullOrWhiteSpace(httpConnectionData.ClientName)
            ? httpClientFactory.CreateClient()
            : httpClientFactory.CreateClient(httpConnectionData.ClientName);

            if (httpConnectionData.Timeout != null)
            {
                httpClient.Timeout = httpConnectionData.Timeout.Value;
            }

            return httpClient;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> SendRequestAsync(
            HttpRequestMessage httpRequestMessage,
            HttpClient httpClient,
            CancellationToken cancellationToken,
            HttpCompletionOption httpCompletionOption = HttpCompletionOption.ResponseContentRead)
        {
            var response = await httpClient.SendAsync(httpRequestMessage, httpCompletionOption, cancellationToken);
            return response;
        }
    }
}
