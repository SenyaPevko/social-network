using Core.Logic.Connections.RabbitMqLogic.Responses.Services;
using IdentityConnectionLib.DtoModels.UserInfoLists;
using Logic.Users.Managers;

namespace Api.Listeners.RabbitMq
{
    /// <summary>
    /// Listenes to rabbit mq with UserInfoListIdentityServiceApiRequest requests
    /// </summary>
    public class UserRabbitMqListener : BackgroundService
    {
        private readonly IRabbitMqResponseService responseService;
        private readonly IServiceScopeFactory serviceScopeFactory;
        public UserRabbitMqListener(IRabbitMqResponseService responseService, IServiceScopeFactory serviceScopeFactory)
        {
            this.responseService = responseService;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// Hadnles request from the RabbitMq queue
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<UserInfoListIdentityServiceApiResponse> HandleRequest(UserInfoListIdentityServiceApiRequest request)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var userList = new List<UserInfo>();
                foreach (var id in request.UsersId)
                {
                    var userLogicManager = scope.ServiceProvider.GetRequiredService<IUserLogicManager>();
                    var user = await userLogicManager.GetUserAsync(id);
                    userList.Add(new UserInfo()
                    {
                        FirstName = user.FirstName,
                        SecondName = user.SecondName
                    });
                }
                var response = new UserInfoListIdentityServiceApiResponse() { UsersInfo = userList.ToArray() };

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

            responseService.StartListeningForRequests<UserInfoListIdentityServiceApiRequest,
                UserInfoListIdentityServiceApiResponse>(HandleRequest);

            return Task.CompletedTask;
        }
    }
}
