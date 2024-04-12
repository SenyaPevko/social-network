namespace IdentityConnectionLib.DtoModels.ProfileInfo;

public record ProfileInfoListIdentityServiceApiRequest : BaseRequest
{
    /// <summary>
    ///     Array of user's ids
    /// </summary>
    public Guid[] UsersId { get; init; }
}