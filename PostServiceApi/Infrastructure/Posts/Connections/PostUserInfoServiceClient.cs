using Domain.Clients.PostUsersInfo;
using IdentityConnectionLib.DtoModels.ProfileInfo;
using IdentityConnectionLib.DtoModels.UserInfoLists;
using IdentityConnectionLib.Services;

namespace Infrastructure.Posts.Connections
{
    public class PostUserInfoServiceClient : IPostUserInfoServiceClient
    {
        private readonly IIdentityConnectionService connectionService;

        public PostUserInfoServiceClient(IIdentityConnectionService connectionService)
        {
            this.connectionService = connectionService;
        }

        public async Task<PostUserInfo[]> GetPostUsersInfoAsync(Guid[] usersId)
        {
            var usersInfoRequest = new UserInfoListIdentityServiceApiRequest()
            {
                UsersId = usersId,
                ResponseAwaitTime = TimeSpan.FromSeconds(3),
                RetryCount = 5,
                RetryInterval = TimeSpan.FromSeconds(3),
            };
            var usersInfo = await connectionService.GetUserInfoListAsync(usersInfoRequest);
            var profilesInfoRequest = new ProfileInfoListIdentityServiceApiRequest()
            {
                UsersId = usersId,
                ResponseAwaitTime = TimeSpan.FromSeconds(3),
                RetryCount = 5,
                RetryInterval = TimeSpan.FromSeconds(3),
            };
            var profilesInfo = await connectionService.GetProfileInfoListAsync(profilesInfoRequest);

            var res = usersInfo.UsersInfo.Zip(profilesInfo.ProfilesInfo, (userInfo, profileInfo) =>
            new PostUserInfo
            {
                FirstName = userInfo.FirstName,
                SecondName = userInfo.SecondName,
                Avatar = profileInfo.Avatar,
                Status = profileInfo.Status,
            });

            return res.ToArray();
        }
    }
}
