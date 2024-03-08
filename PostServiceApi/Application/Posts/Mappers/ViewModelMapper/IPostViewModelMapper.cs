using Application.Interfaces;
using Application.Posts.Models;
using Domain.Posts;

namespace Application.Posts.Mappers.ViewModelMapper
{
    public interface IPostViewModelMapper : IMapper<PostViewModel, Post>
    {
    }
}
