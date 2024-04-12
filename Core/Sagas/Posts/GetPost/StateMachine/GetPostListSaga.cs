using IdentityConnectionLib.DtoModels.ProfileInfo;
using IdentityConnectionLib.DtoModels.UserInfoLists;
using MassTransit;
using PostConnectionLib.DtoModels.PostInfo;
using Sagas.Posts.GetPost.Mappers;
using Sagas.Posts.GetPost.Requests;
using Sagas.Posts.GetPost.Responses;

namespace Sagas.Posts.GetPost.StateMachine;

/// <summary>
///     Represents a saga for processing and retrieving post information along with related user and profile data.
/// </summary>
public class GetPostListSaga : MassTransitStateMachine<GetPostListSagaState>
{
    private readonly IPostViewModelMapper postViewModelMapper;

    /// <summary>
    ///     Initializes a new instance of the GetPostSaga class.
    /// </summary>
    [Obsolete("Obsolete")]
    public GetPostListSaga(IPostViewModelMapper postViewModelMapper)
    {
        this.postViewModelMapper = postViewModelMapper;
        InstanceState(x => x.CurrentState);

        Event(() => GetPostListRequest, x => x.CorrelateById(context => context.Message.CorrelationId));

        Request(() => GetPostInfoList);
        Request(() => GetUserInfoList);
        Request(() => GetProfileInfoList);

        Initially(
            When(GetPostListRequest)
                .Then(context =>
                {
                    if (!context.TryGetPayload(
                            out SagaConsumeContext<GetPostListSagaState, GetPostListRequest>? payload))
                        throw new Exception("Unable to retrieve required payload for callback data.");
                    context.Saga.RequestId = payload.RequestId;
                    context.Saga.ResponseAddress = payload.ResponseAddress;
                })
                .Then(HandlePostListRequest)
                .Request(GetPostInfoList, context => PostInfoRequest)
                .TransitionTo(GetPostInfoList?.Pending),
            When(GetPostInfoList?.Completed)
                .Then(HandlePostInfoReceived)
                .Request(GetUserInfoList, context => UserInfoRequest)
                .TransitionTo(GetUserInfoList?.Pending),
            When(GetPostInfoList?.Faulted)
                .ThenAsync(async context => await Respond(context))
                .TransitionTo(Failed),
            When(GetPostInfoList?.TimeoutExpired)
                .ThenAsync(async context => { await Respond(context); })
                .TransitionTo(Failed),
            When(GetUserInfoList?.Completed)
                .Then(HandleUserInfoReceived)
                .Request(GetProfileInfoList, context => ProfileInfoRequest)
                .TransitionTo(GetProfileInfoList?.Pending),
            When(GetProfileInfoList?.Faulted)
                .ThenAsync(async context => await Respond(context))
                .TransitionTo(Failed),
            When(GetProfileInfoList?.TimeoutExpired)
                .ThenAsync(async context => { await Respond(context); })
                .TransitionTo(Failed),
            When(GetProfileInfoList?.Completed)
                .Then(HandleProfileInfoReceived)
                .ThenAsync(async context => { await Respond(context); })
                .Finalize()
        );

        SetCompletedWhenFinalized();
    }

    /// <summary>
    ///     The request to retrieve post information from the post service.
    /// </summary>
    public Request<GetPostListSagaState, PostInfoListPostServiceApiRequest, PostInfoListPostServiceApiResponse>
        GetPostInfoList { get; set; }

    /// <summary>
    ///     The request to retrieve profile information from the identity service.
    /// </summary>
    public Request<GetPostListSagaState, ProfileInfoListIdentityServiceApiRequest,
            ProfileInfoListIdentityServiceApiResponse>
        GetProfileInfoList { get; set; }

    /// <summary>
    ///     The request to retrieve user information from the identity service.
    /// </summary>
    public Request<GetPostListSagaState, UserInfoListIdentityServiceApiRequest, UserInfoListIdentityServiceApiResponse>
        GetUserInfoList { get; set; }

    /// <summary>
    ///     The response containing user information retrieved from the identity service.
    /// </summary>
    private UserInfoListIdentityServiceApiResponse UserInfoResponse { get; set; } = new();

    /// <summary>
    ///     The response containing profile information retrieved from the identity service.
    /// </summary>
    private ProfileInfoListIdentityServiceApiResponse ProfileInfoResposne { get; set; } = new();

    /// <summary>
    ///     The response containing post information retrieved from the post service.
    /// </summary>
    private PostInfoListPostServiceApiResponse PostInfoResponse { get; set; } = new();

    /// <summary>
    ///     The request sent to the identity service to retrieve user information.
    /// </summary>
    private UserInfoListIdentityServiceApiRequest UserInfoRequest { get; set; } = new();

    /// <summary>
    ///     The request sent to the identity service to retrieve profile information.
    /// </summary>
    private ProfileInfoListIdentityServiceApiRequest ProfileInfoRequest { get; set; } = new();

    /// <summary>
    ///     The request sent to the post service to retrieve post information.
    /// </summary>
    private PostInfoListPostServiceApiRequest PostInfoRequest { get; set; } = new();

    /// <summary>
    ///     Represents the state for failed processing.
    /// </summary>
    public State Failed { get; set; }

    /// <summary>
    ///     Event triggered when a request for post information is received.
    /// </summary>
    public Event<GetPostListRequest> GetPostListRequest { get; set; }

    /// <summary>
    ///     Handles the processing of a <see cref="GetPostListRequest" /> by initializing the request instance.
    /// </summary>
    /// <param name="context">The behavior context containing the current saga state and incoming request data.</param>
    [Obsolete("Obsolete")]
    private void HandlePostListRequest(
        BehaviorContext<GetPostListSagaState, GetPostListRequest> context)
    {
        PostInfoRequest = new PostInfoListPostServiceApiRequest
        {
            PostsId = context.Data.PostsId
        };
    }

    /// <summary>
    ///     Handles the processing of a <see cref="PostInfoListPostServiceApiResponse" /> by populating the response instance
    ///     and preparing subsequent requests for user and profile information.
    /// </summary>
    /// <param name="context">The behavior context containing the current saga state and incoming response data.</param>
    [Obsolete("Obsolete")]
    private void HandlePostInfoReceived(
        BehaviorContext<GetPostListSagaState, PostInfoListPostServiceApiResponse> context)
    {
        var usersIds = context.Data.PostsInfo.Select(post => post.UserId).ToArray();

        PostInfoResponse = new PostInfoListPostServiceApiResponse
        {
            PostsInfo = context.Data.PostsInfo
        };
        UserInfoRequest = new UserInfoListIdentityServiceApiRequest
        {
            UsersId = usersIds
        };
        ProfileInfoRequest = new ProfileInfoListIdentityServiceApiRequest
        {
            UsersId = usersIds
        };
    }

    /// <summary>
    ///     Handles the processing of a <see cref="UserInfoListIdentityServiceApiResponse" /> by populating the response
    ///     instance.
    /// </summary>
    /// <param name="context">The behavior context containing the current saga state and incoming response data.</param>
    [Obsolete("Obsolete")]
    private void HandleUserInfoReceived(
        BehaviorContext<GetPostListSagaState, UserInfoListIdentityServiceApiResponse> context)
    {
        UserInfoResponse = new UserInfoListIdentityServiceApiResponse
        {
            UsersInfo = context.Data.UsersInfo
        };
    }

    /// <summary>
    ///     Handles the processing of a <see cref="ProfileInfoListIdentityServiceApiResponse" /> by populating the response
    ///     instance.
    /// </summary>
    /// <param name="context">The behavior context containing the current saga state and incoming response data.</param>
    [Obsolete("Obsolete")]
    private void HandleProfileInfoReceived(
        BehaviorContext<GetPostListSagaState, ProfileInfoListIdentityServiceApiResponse> context)
    {
        ProfileInfoResposne = new ProfileInfoListIdentityServiceApiResponse
        {
            ProfilesInfo = context.Data.ProfilesInfo
        };
    }

    /// <summary>
    ///     Sends a response containing post view models mapped from retrieved data.
    /// </summary>
    /// <typeparam name="T">The type of response data.</typeparam>
    /// <param name="context">The behavior context containing the current saga state and incoming data.</param>
    private async Task Respond<T>(BehaviorContext<GetPostListSagaState, T> context)
        where T : class
    {
        if (context.Saga.ResponseAddress != null)
        {
            var endpoint = await context.GetSendEndpoint(context.Saga.ResponseAddress);
            await endpoint.Send(new GetPostListResponse
            {
                CorrelationId = context.Saga.CorrelationId,
                PostViewModels = postViewModelMapper.Map(
                    PostInfoResponse.PostsInfo,
                    ProfileInfoResposne.ProfilesInfo,
                    UserInfoResponse.UsersInfo)
            }, r => r.RequestId = context.Saga.RequestId);
        }
    }
}