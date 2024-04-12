using Application.Posts.Models;
using IdentityConnectionLib.DtoModels.ProfileInfo;
using IdentityConnectionLib.DtoModels.UserInfoLists;
using PostConnectionLib.DtoModels.PostInfo;

namespace Sagas.Posts.GetPost.Mappers;

public interface IPostViewModelMapper
{
    /// <summary>
    ///     Creates a response containing view models based on retrieved post, user, and profile information.
    /// </summary>
    /// <param name="postsInfo"></param>
    /// <param name="profileInfos"></param>
    /// <param name="usersInfo"></param>
    /// <returns>A response containing view models for processed posts.</returns>
    public PostViewModel[] Map(
        IReadOnlyList<PostInfo> postsInfo,
        IReadOnlyList<ProfileInfo> profileInfos,
        IReadOnlyList<UserInfo> usersInfo);
}