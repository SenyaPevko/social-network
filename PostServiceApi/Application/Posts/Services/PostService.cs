using Application.Posts.Mappers.InputModelMapper;
using Application.Posts.Mappers.ViewModelMapper;
using Application.Posts.Models;
using Domain.Clients.PostUsersInfo;
using Domain.Posts;
using Domain.Posts.Exceptions;

namespace Application.Posts.Services
{
    public sealed class PostService : IPostService
    {
        private readonly IPostRepository postRepository;
        private readonly IPostInputModelMapper postInputModelMapper;
        private readonly IPostViewModelMapper postViewModelMapper;
        private readonly IPostUserInfoServiceClient postUserInfoServiceClient;

        public PostService(
            IPostRepository postRepository,
            IPostInputModelMapper postMapper, 
            IPostUserInfoServiceClient postUserInfoServiceClient,
            IPostViewModelMapper postViewModelMapper)
        {
            this.postRepository = postRepository;
            this.postInputModelMapper = postMapper;
            this.postUserInfoServiceClient = postUserInfoServiceClient;
            this.postViewModelMapper = postViewModelMapper;
        }

        public async Task<Guid> CreateAsync(PostInputModel inputModel)
        {
            var entity = postInputModelMapper.Map(inputModel with { Id = Guid.Empty });

            return await postRepository.CreateAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await postRepository.DeleteAsync(id);
        }

        public async Task<PostViewModel> GetAsync(Guid id)
        {
            var entity = await postRepository.GetAsync(id) ?? throw new PostNotFoundException(id);
            var userInfo = await postUserInfoServiceClient.GetPostUsersInfoAsync([id]);
            var viewModel = postViewModelMapper.Map(entity);

            return viewModel with { PostUserInfo = userInfo.First()};
        }

        public async Task<IEnumerable<PostViewModel>> GetPageAsync(int pageNumber, int pageSize)
        {
            var page = await postRepository.GetPageAsync(pageNumber, pageSize);
            var usersId = page.Select(post => post.Id).ToArray();
            var usersInfo = await postUserInfoServiceClient.GetPostUsersInfoAsync(usersId);

            return page.Zip(usersInfo, (post, userInfo) => postViewModelMapper.Map(post) with { PostUserInfo = userInfo});
        }

        public async Task UpdateAsync(PostInputModel inputModel)
        {
            if(await postRepository.GetAsync(inputModel.Id) is null)
                throw new PostNotFoundException(inputModel.Id);

            var entity = postInputModelMapper.Map(inputModel);
            await postRepository.UpdateAsync(entity);
        }
    }
}
