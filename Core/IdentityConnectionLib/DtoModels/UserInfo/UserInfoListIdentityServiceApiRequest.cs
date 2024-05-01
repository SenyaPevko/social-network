namespace IdentityConnectionLib.DtoModels.UserInfoLists;

public record UserInfoListIdentityServiceApiRequest : BaseRequest
{
    /// <summary>
    ///     Array of user's ids
    /// </summary>
    public Guid[] UsersId { get; init; }
}