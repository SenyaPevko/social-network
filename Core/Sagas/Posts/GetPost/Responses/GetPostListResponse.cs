using Application.Posts.Models;

namespace Sagas.Posts.GetPost.Responses;

/// <summary>
///     Represents a GetPostSaga state machine response containing a list of post view models.
/// </summary>
public record GetPostListResponse
{
    /// <summary>
    ///     The array of post view models.
    /// </summary>
    public PostViewModel[] PostViewModels { get; set; }

    /// <summary>
    ///     The correlation ID for the saga instance.
    /// </summary>
    public Guid CorrelationId { get; set; }
}