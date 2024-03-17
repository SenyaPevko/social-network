using Application.Interfaces;
using Application.Posts.Models;
using Application.Tags;
using Application.Tags.Mappers;
using Domain.Posts;
using Domain.Tags;

namespace Application.Posts.Mappers.InputModelMapper
{
    public class PostInputModelMapper : IPostInputModelMapper
    {
        private readonly ITagViewModelMapper tagMapper;

        public PostInputModelMapper(ITagViewModelMapper tagMapper)
        {
            this.tagMapper = tagMapper;
        }

        public PostInputModel Map(Post entity)
        {
            var mappedTags = tagMapper.Map(entity.Tags).ToArray();

            return new PostInputModel
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Content = entity.Content,
                LikesCount = entity.LikesCount,
                CommentsCount = entity.CommentsCount,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Tags = mappedTags
            };
        }

        public IEnumerable<PostInputModel> Map(IEnumerable<Post> entities)
        {
            return entities.Select(Map);
        }

        public IEnumerable<Post> Map(IEnumerable<PostInputModel> inputModels)
        {
            return inputModels.Select(Map);
        }

        public Post Map(PostInputModel inputModel)
        {
            var mappedTagInputModels = tagMapper.Map(inputModel.Tags).ToArray();

            return new Post
            {
                Id = inputModel.Id,
                UserId = inputModel.UserId,
                Content = inputModel.Content,
                LikesCount = inputModel.LikesCount,
                CommentsCount = inputModel.CommentsCount,
                CreatedAt = inputModel.CreatedAt,
                UpdatedAt = inputModel.UpdatedAt,
                Tags = mappedTagInputModels
            };
        }
    }
}
