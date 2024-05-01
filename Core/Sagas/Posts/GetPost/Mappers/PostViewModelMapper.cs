using Application.Posts.Models;
using Application.Tags;
using Domain.Clients.PostUsersInfo;
using IdentityConnectionLib.DtoModels.ProfileInfo;
using IdentityConnectionLib.DtoModels.UserInfoLists;
using PostConnectionLib.DtoModels.PostInfo;

namespace Sagas.Posts.GetPost.Mappers;

public class PostViewModelMapper : IPostViewModelMapper
{
    /// <inheritdoc cref="IPostViewModelMapper" />
    public PostViewModel[] Map(
        IReadOnlyList<PostInfo> postsInfo,
        IReadOnlyList<ProfileInfo> profileInfos,
        IReadOnlyList<UserInfo> usersInfo)
    {
        if (postsInfo == null || profileInfos == null || usersInfo == null)
            return Array.Empty<PostViewModel>();

        var viewModels = postsInfo
            .Zip(profileInfos, (postInfo, profileInfo) => (postInfo, profileInfo))
            .Zip(usersInfo, (pair, userInfo) => (pair.postInfo, pair.profileInfo, userInfo))
            .Select(tuple =>
            {
                var (postInfo, profileInfo, userInfo) = tuple;

                var mappedTags = postInfo.Tags?.Select(tag => new TagViewModel
                {
                    Id = tag.Id,
                    Value = tag.Value
                }).ToArray() ?? Array.Empty<TagViewModel>();

                return new PostViewModel
                {
                    Id = postInfo.Id,
                    UserId = postInfo.UserId,
                    Content = postInfo.Content,
                    LikesCount = postInfo.LikesCount,
                    CommentsCount = postInfo.CommentsCount,
                    CreatedAt = postInfo.CreatedAt,
                    UpdatedAt = postInfo.UpdatedAt,
                    Tags = mappedTags,
                    PostUserInfo = new PostUserInfo
                    {
                        Avatar = profileInfo.Avatar,
                        Status = profileInfo.Status,
                        FirstName = userInfo.FirstName,
                        SecondName = userInfo.SecondName
                    }
                };
            })
            .ToArray();

        return viewModels;
    }
}