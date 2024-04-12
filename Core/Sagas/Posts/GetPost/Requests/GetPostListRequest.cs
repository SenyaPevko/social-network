namespace Sagas.Posts.GetPost.Requests;

/// <summary>
///     Represents a request model for obtaining information about a list of posts.
/// </summary>
public class GetPostListRequest
{
    /// <summary>
    ///     The array of post IDs for which information is requested.
    /// </summary>
    public Guid[] PostsId { get; set; }

    /// <summary>
    ///     The correlation ID for the saga instance.
    /// </summary>
    public Guid CorrelationId { get; set; }
}