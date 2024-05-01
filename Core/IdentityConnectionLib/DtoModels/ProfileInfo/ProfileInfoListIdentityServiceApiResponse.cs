namespace IdentityConnectionLib.DtoModels.ProfileInfo;

public record ProfileInfoListIdentityServiceApiResponse
{
    /// <summary>
    ///     Array of profile's information
    /// </summary>
    public ProfileInfo[] ProfilesInfo { get; init; }
}