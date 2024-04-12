namespace PostConnectionLib.DtoModels.PostInfo;

/// <summary>
/// Represents a request model for obtaining information about a list of posts.
/// </summary>
public record PostInfoListPostServiceApiRequest
{
    /// <summary>
    /// The array of post IDs for which information is requested.
    /// </summary>
    public Guid[] PostsId { get; set; }
}