using IdentityConnectionLib.DtoModels.ProfileInfo;
using Logic.UserProfiles.Managers;
using MassTransit;

namespace Api.Consumers
{
    /// <summary>
    /// Consumer for processing user profile information requests.
    /// </summary>
    public class UserProfileListConsumer : IConsumer<ProfileInfoListIdentityServiceApiRequest>
    {
        private readonly IUserProfileLogicManager userProfileLogicManager;

        /// <summary>
        /// Initializes a new instance of the UserProfileListConsumer class.
        /// </summary>
        /// <param name="userProfileLogicManager">The user profile logic manager used to retrieve user profiles.</param>
        public UserProfileListConsumer(IUserProfileLogicManager userProfileLogicManager)
        {
            this.userProfileLogicManager = userProfileLogicManager;
        }

        /// <summary>
        /// Consumes a user profile information request and responds with the corresponding profile information.
        /// </summary>
        /// <param name="context">The consume context containing the incoming request.</param>
        public async Task Consume(ConsumeContext<ProfileInfoListIdentityServiceApiRequest> context)
        {
            var request = context.Message;
            var profilesList = new List<ProfileInfo>();
            foreach (var id in request.UsersId)
            {
                var profile = await userProfileLogicManager.GetUserProfileByUserIdAsync(id);
                profilesList.Add(new ProfileInfo()
                {
                    Avatar = profile.AvatarUrl,
                    Status = profile.Status
                });
            }

            var response = new ProfileInfoListIdentityServiceApiResponse() { ProfilesInfo = profilesList.ToArray() };

            await context.RespondAsync(response);
        }
    }
}