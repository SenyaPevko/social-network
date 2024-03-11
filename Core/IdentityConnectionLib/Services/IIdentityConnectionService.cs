using IdentityConnectionLib.DtoModels.ProfileInfo;
using IdentityConnectionLib.DtoModels.UserInfoLists;

namespace IdentityConnectionLib.Services;

/// <summary>
///     Service to send requests to IdentityServiceApi
/// </summary>
public interface IIdentityConnectionService
{
    /// <summary>
    ///     Get list of users' info
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<UserInfoListIdentityServiceApiResponse> GetUserInfoListAsync(UserInfoListIdentityServiceApiRequest request);

    /// <summary>
    ///     Get list of profiles' info
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<ProfileInfoListIdentityServiceApiResponse> GetProfileInfoListAsync(
        ProfileInfoListIdentityServiceApiRequest request);
}