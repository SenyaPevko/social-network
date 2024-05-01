namespace PostConnectionLib.DtoModels.PostInfo;

/// <summary>
/// Represents a response model containing information about a list of posts.
/// </summary>
public record PostInfoListPostServiceApiResponse
{
    /// <summary>
    /// Gets or sets the array of post information retrieved from the service.
    /// </summary>
    public PostInfo[] PostsInfo { get; set; }
}