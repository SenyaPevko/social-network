using Core.HttpLogic.Base;
using Core.HttpLogic.HttpRequests;
using Core.HttpLogic.HttpRequests.Models;
using Core.HttpLogic.HttpRequests.Services;
using IdentityConnectionLib.Config.Models;
using IdentityConnectionLib.DtoModels.ProfileInfo;
using IdentityConnectionLib.DtoModels.UserInfoLists;
using IdentityConnectionLib.ResponseHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityConnectionLib.Services;

/// <inheritdoc />
public class IdentityConnectionService : IIdentityConnectionService
{
    private readonly IIdentityApiConnectionConfig configuration;
    private readonly IHttpRequestService httpClientFactory;
    private readonly IResponseHandler responseHandler;

    public IdentityConnectionService(
        IIdentityApiConnectionConfig configuration,
        IServiceProvider serviceProvider,
        IResponseHandler responseHandler)
    {
        this.responseHandler = responseHandler;
        this.configuration = configuration;

        if (configuration.ConnectionType == ConnectionType.Http)
        {
            httpClientFactory = serviceProvider.GetRequiredService<IHttpRequestService>();
        }
        // RPC по rabbit
    }

    /// <inheritdoc />
    public async Task<ProfileInfoListIdentityServiceApiResponse> GetProfileInfoListAsync(
        ProfileInfoListIdentityServiceApiRequest request)
    {
        var requestData = new HttpRequestData
        {
            Method = HttpMethod.Get,
            Uri = new Uri(configuration.ProfilesInfoUri),
            Body = request,
            ContentType = ContentType.ApplicationJson,
            ResponseAwaitTime = request.ResponseAwaitTime,
            RetryCount = request.RetryCount,
            RetryInterval = request.RetryInterval
        };

        var response = await httpClientFactory.SendRequestAsync<ProfileInfoListIdentityServiceApiResponse>(requestData);
        if (!response.IsSuccessStatusCode)
            responseHandler.HandleErrorResponse(response.StatusCode);

        return response.Body;
    }

    /// <inheritdoc />
    public async Task<UserInfoListIdentityServiceApiResponse> GetUserInfoListAsync(
        UserInfoListIdentityServiceApiRequest request)
    {
        var requestData = new HttpRequestData
        {
            Method = HttpMethod.Get,
            Uri = new Uri(configuration.UsersInfoUri),
            Body = request,
            ContentType = ContentType.ApplicationJson,
            ResponseAwaitTime = request.ResponseAwaitTime,
            RetryCount = request.RetryCount,
            RetryInterval = request.RetryInterval
        };

        var response = await httpClientFactory.SendRequestAsync<UserInfoListIdentityServiceApiResponse>(requestData);
        if (!response.IsSuccessStatusCode)
            responseHandler.HandleErrorResponse(response.StatusCode);

        return response.Body;
    }
}