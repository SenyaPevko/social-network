using IdentityConnectionLib.DtoModels.UserInfoLists;
using Logic.Users.Managers;
using MassTransit;

namespace Api.Consumers
{
    /// <summary>
    /// Consumer for processing user information requests.
    /// </summary>
    public class UserInfoListConsumer : IConsumer<UserInfoListIdentityServiceApiRequest>
    {
        private readonly IUserLogicManager userLogicManager;

        /// <summary>
        /// Initializes a new instance of the UserInfoListConsumer class.
        /// </summary>
        /// <param name="userLogicManager">The user logic manager used to retrieve user information.</param>
        public UserInfoListConsumer(IUserLogicManager userLogicManager)
        {
            this.userLogicManager = userLogicManager;
        }

        /// <summary>
        /// Consumes a user information request and responds with the corresponding user information.
        /// </summary>
        /// <param name="context">The consume context containing the incoming request.</param>
        public async Task Consume(ConsumeContext<UserInfoListIdentityServiceApiRequest> context)
        {
            var request = context.Message;
            var userList = new List<UserInfo>();
            foreach (var id in request.UsersId)
            {
                var user = await userLogicManager.GetUserAsync(id);
                userList.Add(new UserInfo()
                {
                    FirstName = user.FirstName,
                    SecondName = user.SecondName
                });
            }

            var response = new UserInfoListIdentityServiceApiResponse() { UsersInfo = userList.ToArray() };

            await context.RespondAsync(response);
        }
    }
}