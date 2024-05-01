using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sagas.Posts.GetPost.Mappers;
using Sagas.Posts.GetPost.StateMachine;

namespace Sagas.Posts.GetPost;

/// <summary>
///     Provides extension methods to configure services required for the GetPostSaga.
/// </summary>
public static class GetPostSagaStartup
{
    /// <summary>
    ///     Adds required services for the GetPostSaga to the specified <paramref name="services" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the services to.</param>
    /// <returns>The modified <see cref="IServiceCollection" /> instance.</returns>
    [Obsolete("Obsolete")]
    public static IServiceCollection AddGetPostSagaServices(this IServiceCollection services, IConfiguration configuration)
    {
        AddMappers(services);
        ConfigureMassTransit(services, configuration);

        return services;
    }

    /// <summary>
    ///     Configures MassTransit with the necessary settings for the GetPostSaga.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to configure MassTransit with.</param>
    [Obsolete("Obsolete")]
    private static void ConfigureMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        var hostName = configuration["RabbitMqConnection:HostName"];
        var port = configuration.GetValue<ushort>("RabbitMqConnection:Port");
        var userName = configuration["RabbitMqConnection:UserName"];
        var password = configuration["RabbitMqConnection:Password"];
        var virtualHost = configuration["RabbitMqConnection:VirtualHost"];
        
        services.AddMassTransit(cfg =>
        {
            cfg.SetKebabCaseEndpointNameFormatter();
            cfg.AddDelayedMessageScheduler();
            cfg.AddSagaStateMachine<GetPostListSaga, GetPostListSagaState>()
                .EntityFrameworkRepository(r =>
                {
                    r.ConcurrencyMode = ConcurrencyMode.Pessimistic;
                    r.ExistingDbContext<GetPostSagaDbContext>();
                    r.LockStatementProvider = new PostgresLockStatementProvider();
                });
            cfg.UsingRabbitMq((brc, rbfc) =>
            {
                rbfc.UseInMemoryOutbox();
                rbfc.UseMessageRetry(r => { r.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)); });
                rbfc.UseDelayedMessageScheduler();
                rbfc.Host(hostName, port, virtualHost, h =>
                {
                    h.Username(userName);
                    h.Password(password);
                });
                rbfc.ConfigureEndpoints(brc);
            });
        });
    }

    /// <summary>
    ///     Adds mappers required for the GetPostSaga to the specified <paramref name="services" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the mappers to.</param>
    private static void AddMappers(this IServiceCollection services)
    {
        services.AddSingleton<IPostViewModelMapper, PostViewModelMapper>();
    }

    /// <summary>
    ///     Configures the database context for the GetPostSaga with the specified database connection string.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to configure.</param>
    /// <param name="connection">The database connection string.</param>
    public static void ConfigureGetPostListSagaDb(this IServiceCollection services, string connection)
    {
        services.AddDbContext<GetPostSagaDbContext>(options => options.UseNpgsql(connection));
    }
}