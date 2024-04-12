namespace IdentityConnectionLib.DtoModels.UserInfoLists;

public record UserInfoListIdentityServiceApiResponse
{
    /// <summary>
    ///     Array of user's information
    /// </summary>
    public UserInfo[] UsersInfo { get; init; }
}