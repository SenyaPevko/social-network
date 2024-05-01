using LockerLib.Clients;
using LockerLib.Helpers.PathHelpers;
using LockerLib.Helpers.WaitHelpers;
using LockerLib.Locks;
using LockerLib.Locks.DistributedLocks;
using LockerLib.Locks.DistributedSemaphores;
using LockerLib.Locks.LockManagers;
using Microsoft.Extensions.DependencyInjection;

namespace LockerLib;

/// <summary>
/// Configures the Zookeeper distributed semaphore in the service collection.
/// </summary>
public static class LockerLibStartup
{
    /// <summary>
    /// Adds the Zookeeper distributed semaphore to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the Zookeeper distributed semaphore to.</param>
    /// <param name="zookeeperLockOptions">The options for configuring the Zookeeper lock.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddZookeeperDistributedSemaphore(this IServiceCollection services,
        ZookeeperLockOptions zookeeperLockOptions)
    {
        AddHelpers(services);
        AddConfiguration(services, zookeeperLockOptions);
        services.AddScoped<ILockerClient, ZooKeeperClient>();
        services.AddScoped<IDistributedLock, ZookeeperDistributedLock>();
        services.AddScoped<ILockManager, ZookeeperLockManager>();
        services.AddScoped<IDistributedSemaphore, ZookeeperDistributedSemaphore>();
        return services;
    }

    private static void AddHelpers(this IServiceCollection services)
    {
        services.AddSingleton<IPathHelper, ZookeeperPathHelper>();
        services.AddSingleton<IWaitHelper, WaitHelper>();
    }

    private static void AddConfiguration(this IServiceCollection services, ZookeeperLockOptions zookeeperLockOptions)
    {
        services.AddSingleton<ZookeeperClientOptions, ZookeeperClientOptions>(_ => new ZookeeperClientOptions
        {
            ConnectionString = zookeeperLockOptions.ConnectionString,
            SessionTimeout = zookeeperLockOptions.SessionTimeout
        });

        services.AddSingleton<ZookeeperLockerPathConfiguration, ZookeeperLockerPathConfiguration>(_ =>
            new ZookeeperLockerPathConfiguration(zookeeperLockOptions.SemaphorePath));

        services.AddSingleton<DistributedSemaphoreConfiguration, DistributedSemaphoreConfiguration>(_ =>
            new DistributedSemaphoreConfiguration(zookeeperLockOptions.SemaphoreMaxCount,
                zookeeperLockOptions.SemaphoreName));
        
        services.AddSingleton<LockManagerConfiguration, LockManagerConfiguration>(_ =>
            new LockManagerConfiguration(zookeeperLockOptions.SemaphoreMaxCount));
    }
}