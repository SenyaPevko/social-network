using Core.Logic.Connections.RabbitMqLogic.Responses.Services;
using IdentityConnectionLib.DtoModels.ProfileInfo;
using Logic.UserProfiles.Managers;

namespace Api.Listeners.RabbitMq
{
    /// <summary>
    /// Listenes to rabbit mq with ProfileInfoListIdentityServiceApiRequest requests
    /// </summary>
    public class UserProfileRabittMqListener : BackgroundService
    {
        private readonly IRabbitMqResponseService responseService; 
        private readonly IServiceScopeFactory serviceScopeFactory;
        public UserProfileRabittMqListener(IRabbitMqResponseService responseService, IServiceScopeFactory serviceScopeFactory)
        {
            this.responseService = responseService;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// Hadnles request from the RabbitMq queue
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<ProfileInfoListIdentityServiceApiResponse> HandleRequest(ProfileInfoListIdentityServiceApiRequest request)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var profilesList = new List<ProfileInfo>();
                foreach (var id in request.UsersId)
                {
                    var userProfileLogicManager = scope.ServiceProvider.GetRequiredService<IUserProfileLogicManager>();
                    var profile = await userProfileLogicManager.GetUserProfileByUserIdAsync(id);
                    profilesList.Add(new ProfileInfo()
                    {
                        Avatar = profile.AvatarUrl,
                        Status = profile.Status
                    });
                }
                var response = new ProfileInfoListIdentityServiceApiResponse() { ProfilesInfo = profilesList.ToArray() };

                return response;
            }
        }

        /// <summary>
        /// Listens to the RabbitMq queue and handles request
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            responseService.StartListeningForRequests<ProfileInfoListIdentityServiceApiRequest, 
                ProfileInfoListIdentityServiceApiResponse>(HandleRequest);

            return Task.CompletedTask;
        }
    }
}
