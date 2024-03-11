using IdentityConnectionLib.DtoModels.UserInfoLists;
using Core.HttpLogic.HttpRequests.Services;
using Microsoft.Extensions.DependencyInjection;
using Core.HttpLogic.HttpRequests.Models;
using Core.HttpLogic.HttpRequests;
using IdentityConnectionLib.DtoModels.ProfileInfo;
using IdentityConnectionLib.ResponseHandlers;
using IdentityConnectionLib.Config.Models;
using IdentityConnectionLib.Config;
using Core.HttpLogic.Base;

namespace IdentityConnectionLib.Services
{
    /// <inheritdoc />
    public class IdentityConnectionService : IIdentityConnectionService
    {
        private readonly IHttpRequestService httpClientFactory;
        private readonly IResponseHandler responseHandler;
        private readonly IIdentityApiConnectionConfig configuration;

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
            else
            {
                // RPC по rabbit
            }
        }

        /// <inheritdoc />
        public async Task<ProfileInfoListIdentityServiceApiResponse> GetProfileInfoListAsync(
            ProfileInfoListIdentityServiceApiRequest request)
        {
            var requestData = new HttpRequestData()
            {
                Method = HttpMethod.Get,
                Uri = new Uri(configuration.ProfilesInfoUri),
                Body = request,
                ContentType = ContentType.ApplicationJson,
                ResponseAwaitTime = request.ResponseAwaitTime,
                RetryCount = request.RetryCount,
                RetryInterval = request.RetryInterval,
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
            var requestData = new HttpRequestData()
            {
                Method = HttpMethod.Get,
                Uri = new Uri(configuration.UsersInfoUri),
                Body = request,
                ContentType = ContentType.ApplicationJson,
                ResponseAwaitTime = request.ResponseAwaitTime,
                RetryCount = request.RetryCount,
                RetryInterval = request.RetryInterval,
            };

            var response = await httpClientFactory.SendRequestAsync<UserInfoListIdentityServiceApiResponse>(requestData);
            if (!response.IsSuccessStatusCode)
                responseHandler.HandleErrorResponse(response.StatusCode);

            return response.Body;
        }
    }
}
