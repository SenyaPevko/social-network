namespace Domain.Clients.PostUsersInfo
{
    public interface IPostUserInfoServiceClient
    {
        Task<PostUserInfo[]> GetPostUsersInfoAsync(Guid[] usersId);
    }
}
