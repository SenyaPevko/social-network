using Core.Logic.Connections.HttpLogic.HttpRequests;
using Core.Logic.Connections.HttpLogic.HttpRequests.Models;
using Core.Logic.Connections.HttpLogic.HttpRequests.Services;
using IdentityConnectionLib.Config.Models;
using IdentityConnectionLib.DtoModels.ProfileInfo;
using IdentityConnectionLib.DtoModels.UserInfoLists;
using IdentityConnectionLib.ResponseHandlers;

namespace IdentityConnectionLib.Services.Http
{
    /// <inheritdoc />
    internal class HttpConnectionService : IHttpConnectionService
    {
        private readonly IHttpRequestService httpRequestService;
        private readonly IResponseHandler responseHandler;
        private readonly IIdentityApiConnectionConfig configuration;
        public HttpConnectionService(
            IHttpRequestService httpRequestService,
            IResponseHandler responseHandler,
            IIdentityApiConnectionConfig configuration)
        {
            this.httpRequestService = httpRequestService;
            this.responseHandler = responseHandler;
            this.configuration = configuration;
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

            var response = await httpRequestService.SendRequestAsync<ProfileInfoListIdentityServiceApiResponse>(requestData);
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

            var response = await httpRequestService.SendRequestAsync<UserInfoListIdentityServiceApiResponse>(requestData);
            if (!response.IsSuccessStatusCode)
                responseHandler.HandleErrorResponse(response.StatusCode);

            return response.Body;
        }
    }
}
