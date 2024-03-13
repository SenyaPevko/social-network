using Core.HttpLogic.HttpConnections.Models;
using Core.HttpLogic.Polly;
using Polly;

namespace Core.HttpLogic.HttpConnections.Services;

/// <inheritdoc />
internal class HttpConnectionService : IHttpConnectionService
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IHttpPolicy httpPolicy;

    public HttpConnectionService(IHttpClientFactory httpClientFactory, IHttpPolicy httpPolicy)
    {
        this.httpClientFactory = httpClientFactory;
        this.httpPolicy = httpPolicy;
    }

    /// <inheritdoc />
    public HttpClient CreateHttpClient(HttpConnectionData httpConnectionData)
    {
        var httpClient = string.IsNullOrWhiteSpace(httpConnectionData.ClientName)
            ? httpClientFactory.CreateClient()
            : httpClientFactory.CreateClient(httpConnectionData.ClientName);

        if (httpConnectionData.Timeout != null) httpClient.Timeout = httpConnectionData.Timeout.Value;

        return httpClient;
    }

    /// <inheritdoc />
    public async Task<HttpResponseMessage> SendRequestAsync(
        HttpRequestMessage httpRequestMessage,
        HttpClient httpClient,
        CancellationToken cancellationToken,
        HttpCompletionOption httpCompletionOption = HttpCompletionOption.ResponseContentRead)
    {
        var response = await httpClient
            .SendAsync(httpRequestMessage, httpCompletionOption, cancellationToken);

        return response;
    }
}