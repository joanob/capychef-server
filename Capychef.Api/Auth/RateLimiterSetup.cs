using RedisRateLimiting;
using StackExchange.Redis;

namespace Capychef.Api.Auth;

public static class RateLimiterSetup
{
    public static void SetupRateLimiter(IServiceCollection services, string redisConnectionString)
    {
        var multiplexer = ConnectionMultiplexer.Connect(redisConnectionString);

        services.AddSingleton<IConnectionMultiplexer>(multiplexer);
        services.AddSingleton<ILoginAttemptTracker, RedisLoginAttemptTracker>();

        services.AddRateLimiter(options =>
        {
            options.AddPolicy(RateLimiterPolicies.Signup, httpContext =>
                RedisRateLimitPartition.GetFixedWindowRateLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new RedisFixedWindowRateLimiterOptions
                    {
                        PermitLimit = 3,
                        Window = TimeSpan.FromHours(1),
                        ConnectionMultiplexerFactory = () => multiplexer
                    }));

            options.AddPolicy(RateLimiterPolicies.UsernameCheck, httpContext =>
                RedisRateLimitPartition.GetFixedWindowRateLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new RedisFixedWindowRateLimiterOptions
                    {
                        PermitLimit = 20,
                        Window = TimeSpan.FromMinutes(1),
                        ConnectionMultiplexerFactory = () => multiplexer
                    }));

            options.RejectionStatusCode = 429;
        });
    }
}

