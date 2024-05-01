using Domain.Posts;
using MassTransit;
using PostConnectionLib.DtoModels.PostInfo;

namespace Infrastructure.Posts.Consumers
{
    /// <summary>
    /// Consumer for processing post information requests.
    /// </summary>
    public class PostInfoListConsumer : IConsumer<PostInfoListPostServiceApiRequest>
    {
        private readonly IPostRepository postRepository;

        /// <summary>
        /// Initializes a new instance of the PostInfoListConsumer class.
        /// </summary>
        /// <param name="repository">The repository for accessing post data.</param>
        public PostInfoListConsumer(IPostRepository repository)
        {
            postRepository = repository;
        }

        /// <summary>
        /// Consumes a post information request and responds with the requested post information.
        /// </summary>
        /// <param name="context">The consume context containing the incoming request.</param>
        public async Task Consume(ConsumeContext<PostInfoListPostServiceApiRequest> context)
        {
            var request = context.Message;
            var infoList = new List<PostInfo>();
            foreach (var id in request.PostsId)
            {
                var post = await postRepository.GetAsync(id);
                var info = new PostInfo()
                {
                    Id = post.Id,
                    UserId = post.UserId,
                    Content = post.Content,
                    LikesCount = post.LikesCount,
                    CommentsCount = post.CommentsCount,
                    CreatedAt = post.CreatedAt,
                    UpdatedAt = post.UpdatedAt,
                    Tags = post.Tags
                };
                infoList.Add(info);
            }

            var response = new PostInfoListPostServiceApiResponse() { PostsInfo = infoList.ToArray() };

            await context.RespondAsync(response);
        }
    }
}