using IdentityConnectionLib.DtoModels.UserInfoLists;
using Core.HttpLogic.HttpRequests.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Core.HttpLogic.HttpRequests.Models;
using Core.HttpLogic.HttpRequests;
using IdentityConnectionLib.DtoModels.ProfileInfo;
using IdentityConnectionLib.ResponseHandlers;

namespace IdentityConnectionLib.Services
{
    public class IdentityConnectionService : IIdentityConnectionService
    {
        private readonly IHttpRequestService httpClientFactory;
        private readonly IResponseHandler responseHandler;

        public IdentityConnectionService(IConfiguration configuration, IServiceProvider serviceProvider, IResponseHandler responseHandler)
        {
            this.responseHandler = responseHandler;

            if (configuration.GetSection("ServiceConnection").Value == "http")
            {
                httpClientFactory = serviceProvider.GetRequiredService<IHttpRequestService>();
            }
            else
            {
                // RPC по rabbit
            }
        }

        public async Task<ProfileInfoListIdentityServiceApiResponse> GetProfileInfoListAsync(ProfileInfoListIdentityServiceApiRequest request)
        {
            var requestData = new HttpRequestData()
            {
                Method = HttpMethod.Get,
                Uri = new Uri("/api/users/profiles"),
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

        public async Task<UserInfoListIdentityServiceApiResponse> GetUserInfoListAsync(UserInfoListIdentityServiceApiRequest request)
        {
            var requestData = new HttpRequestData()
            {
                Method = HttpMethod.Get,
                Uri = new Uri("/api/users/list"),
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
