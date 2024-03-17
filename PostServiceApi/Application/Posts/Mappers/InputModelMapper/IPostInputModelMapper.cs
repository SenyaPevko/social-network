using Application.Interfaces;
using Application.Posts.Models;
using Domain.Posts;

namespace Application.Posts.Mappers.InputModelMapper
{
    public interface IPostInputModelMapper : IMapper<PostInputModel, Post>
    {
    }
}
