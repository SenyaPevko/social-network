using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Sagas.Posts.GetPost.StateMaps;

namespace Sagas.Posts;

/// <summary>
///     Represents the database context for managing sagas related to getting post information.
/// </summary>
public sealed class GetPostSagaDbContext : SagaDbContext
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="GetPostSagaDbContext" /> class with the specified options.
    /// </summary>
    /// <param name="options">The DbContext options.</param>
    public GetPostSagaDbContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    /// <summary>
    ///     Gets the saga class map configurations for the database context.
    /// </summary>
    protected override IEnumerable<ISagaClassMap> Configurations => new ISagaClassMap[]
    {
        new GetPostListSagaStateMap()
    };
}