namespace IdentityConnectionLib.DtoModels.ProfileInfo;

public record ProfileInfo
{
    /// <summary>
    ///     Status that user set in their profile
    /// </summary>
    public required string Status { get; init; }

    /// <summary>
    ///     Avatar that user set in their profile
    /// </summary>
    public required string Avatar { get; init; }
}