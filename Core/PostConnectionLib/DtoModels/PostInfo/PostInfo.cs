using Domain.Tags;

namespace PostConnectionLib.DtoModels.PostInfo;

/// <summary>
/// Represents info about the post
/// </summary>
public record PostInfo
{
    /// <summary>
    /// Id of this post
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Id of the user that created this post
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Post content: text/images/videos etc
    /// </summary>
    public string Content { get; init; } = null!;

    /// <summary>
    /// Amount of likes on the post
    /// </summary>
    public int LikesCount {  get; init; }

    /// <summary>
    /// Amount of comments on the post
    /// </summary>
    public int CommentsCount { get; init; }

    /// <summary>
    /// The date and time when the post was created
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The date and time when the post was last updated
    /// </summary>
    public DateTime UpdatedAt { get; init; }

    /// <summary>
    /// The tags that were asigned to the post
    /// </summary>
    public ICollection<Tag> Tags { get; init; } = null!;
}