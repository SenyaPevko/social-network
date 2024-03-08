using Application.Interfaces.Services;
using Application.Posts.Models;

namespace Application.Posts.Services
{
    public interface IPostService :
        ICreateService<PostInputModel>,
        IUpdateService<PostInputModel>,
        IDeleteService<PostInputModel>,
        IGetPageService<PostViewModel>,
        IGetService<PostViewModel>
    {
    }
}
