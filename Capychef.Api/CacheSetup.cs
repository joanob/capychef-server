using Capychef.Api.Auth;
using Capychef.Households.Domain.Interfaces;

namespace Capychef.Api;

public static class CacheSetup
{
    public static void SetupCache(IServiceCollection services, IWebHostEnvironment environment)
    {
        if (environment.IsDev())
            SetupDevCache(services);
        else
            SetupProductionCache(services);
    }

    private static void SetupDevCache(IServiceCollection services)
    {
        services.AddSingleton<IMembershipCache, NoOpMembershipCache>();
    }

    private static void SetupProductionCache(IServiceCollection services)
    {
        var redisConnectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING")
                                    ?? throw new InvalidOperationException(
                                        "REDIS_CONNECTION_STRING environment variable not found");

        var multiplexer = ConnectionMultiplexer.Connect(redisConnectionString);

        services.AddSingleton<IMembershipCache>(_ => new RedisMembershipCache(multiplexer));
    }
}