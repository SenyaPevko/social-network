namespace IdentityConnectionLib.DtoModels.UserInfoLists;

public record UserInfoListIdentityServiceApiResponse
{
    /// <summary>
    ///     Array of user's information
    /// </summary>
    public required UserInfo[] UsersInfo { get; init; }
}