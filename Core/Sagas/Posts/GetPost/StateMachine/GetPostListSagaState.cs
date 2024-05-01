using MassTransit;

namespace Sagas.Posts.GetPost.StateMachine;

/// <summary>
///     Represents the state instance for the GetPostSaga state machine.
/// </summary>
public class GetPostListSagaState : SagaStateMachineInstance
{
    /// <summary>
    ///     The ID of the request associated with the saga instance.
    /// </summary>
    public Guid? RequestId { get; set; }

    /// <summary>
    ///     The response address associated with the saga instance.
    /// </summary>
    public Uri? ResponseAddress { get; set; }

    /// <summary>
    ///     The current state of the saga instance.
    /// </summary>
    public State? CurrentState { get; set; }

    /// <summary>
    ///     The correlation ID for the saga instance.
    /// </summary>
    public Guid CorrelationId { get; set; }
}