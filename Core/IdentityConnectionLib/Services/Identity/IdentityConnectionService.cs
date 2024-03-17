using Core.Logic.Connections.Base;
using IdentityConnectionLib.Config.Models;
using IdentityConnectionLib.DtoModels.ProfileInfo;
using IdentityConnectionLib.DtoModels.UserInfoLists;
using IdentityConnectionLib.Services.Http;
using IdentityConnectionLib.Services.RabbitMq;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityConnectionLib.Services.Identity
{
    /// <inheritdoc />
    public class IdentityConnectionService : IIdentityConnectionService
    {
        private readonly IIdentityApiConnectionConfig configuration;
        private readonly IIdentityConnectionService connectionService;

        public IdentityConnectionService(IIdentityApiConnectionConfig configuration, IServiceProvider serviceProvider)
        {
            this.configuration = configuration;

            switch (configuration.ConnectionType)
            {
                case ConnectionType.Http:
                    connectionService = serviceProvider.GetRequiredService<IHttpConnectionService>();
                    break;
                default:
                    connectionService = serviceProvider.GetRequiredService<IRabbitMqConnectionService>();
                    break;
            }
        }

        /// <inheritdoc />
        public async Task<ProfileInfoListIdentityServiceApiResponse> GetProfileInfoListAsync(
            ProfileInfoListIdentityServiceApiRequest request)
        {
            return await connectionService.GetProfileInfoListAsync(request);
        }

        /// <inheritdoc />
        public async Task<UserInfoListIdentityServiceApiResponse> GetUserInfoListAsync(
            UserInfoListIdentityServiceApiRequest request)
        {
            return await connectionService.GetUserInfoListAsync(request);
        }
    }
}