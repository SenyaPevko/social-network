using Core.Logic.Connections.RabbitMqLogic.Requests.Services;
using IdentityConnectionLib.DtoModels.ProfileInfo;
using IdentityConnectionLib.DtoModels.UserInfoLists;

namespace IdentityConnectionLib.Services.RabbitMq
{
    /// <inheritdoc />
    internal class RabbitMqConnectionService : IRabbitMqConnectionService
    {
        private readonly IRabbitMqRequestService rabbitMqRequestService;

        public RabbitMqConnectionService(IRabbitMqRequestService rabbitMqRequestService)
        {
            this.rabbitMqRequestService = rabbitMqRequestService;
        }

        /// <inheritdoc />
        public async Task<ProfileInfoListIdentityServiceApiResponse> GetProfileInfoListAsync(
            ProfileInfoListIdentityServiceApiRequest request)
        {
            var response = await rabbitMqRequestService.SendRequestAsync<ProfileInfoListIdentityServiceApiRequest,
                ProfileInfoListIdentityServiceApiResponse>(request);

            return response;
        }

        /// <inheritdoc />
        public async Task<UserInfoListIdentityServiceApiResponse> GetUserInfoListAsync(
            UserInfoListIdentityServiceApiRequest request)
        {
            var response = await rabbitMqRequestService.SendRequestAsync<UserInfoListIdentityServiceApiRequest,
                UserInfoListIdentityServiceApiResponse>(request);

            return response;
        }
    }
}
