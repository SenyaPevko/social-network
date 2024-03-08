using IdentityConnectionLib.DtoModels.ProfileInfo;
using IdentityConnectionLib.DtoModels.UserInfoLists;

namespace IdentityConnectionLib.Services
{
    internal interface IIdentityConnectionService
    {
        Task<UserInfoListIdentityServiceApiResponse> GetUserInfoListAsync(UserInfoListIdentityServiceApiRequest request);
        Task<ProfileInfoListIdentityServiceApiResponse> GetProfileInfoListAsync(ProfileInfoListIdentityServiceApiRequest request);
    }
}
