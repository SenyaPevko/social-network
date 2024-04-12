using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sagas.Posts.GetPost.StateMachine;

namespace Sagas.Posts.GetPost.StateMaps;

/// <summary>
///     Represents the mapping configuration for the state of the GetPostListSaga saga.
/// </summary>
public class GetPostListSagaStateMap : SagaClassMap<GetPostListSagaState>
{
    /// <summary>
    ///     Configures the entity mapping for the saga state.
    /// </summary>
    /// <param name="entity">The entity type builder.</param>
    /// <param name="model">The model builder.</param>
    protected override void Configure(EntityTypeBuilder<GetPostListSagaState> entity, ModelBuilder model)
    {
        entity.HasIndex(x => x.CorrelationId).IsUnique();
        entity.Ignore(x => x.CurrentState);
        entity.Property(x => x.RequestId).IsRequired();
        entity.Property(x => x.ResponseAddress).IsRequired();
    }
}