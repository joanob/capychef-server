using System.Threading.RateLimiting;
using RedisRateLimiting;
using StackExchange.Redis;

namespace Capychef.Api.Auth;

public static class RateLimiterSetup
{
    public static void SetupRateLimiter(IServiceCollection services, IWebHostEnvironment environment)
    {
        if (environment.IsDev())
            SetupDevRateLimiter(services);
        else
            SetupProductionRateLimiter(services);
    }

    private static void SetupDevRateLimiter(IServiceCollection services)
    {
        services.AddSingleton<ILoginAttemptTracker, NoOpLoginAttemptTracker>();

        services.AddRateLimiter(options =>
        {
            options.AddPolicy(RateLimiterPolicies.Signup, _ =>
                RateLimitPartition.GetNoLimiter(RateLimiterPolicies.Signup));

            options.AddPolicy(RateLimiterPolicies.UsernameCheck, _ =>
                RateLimitPartition.GetNoLimiter(RateLimiterPolicies.UsernameCheck));

            options.AddPolicy(RateLimiterPolicies.EmailValidation, _ =>
                RateLimitPartition.GetNoLimiter(RateLimiterPolicies.EmailValidation));

            options.AddPolicy(RateLimiterPolicies.PasswordRecovery, _ =>
                RateLimitPartition.GetNoLimiter(RateLimiterPolicies.PasswordRecovery));
        });
    }

    private static void SetupProductionRateLimiter(IServiceCollection services)
    {
        var redisConnectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING")
                                    ?? throw new InvalidOperationException(
                                        "REDIS_CONNECTION_STRING environment variable not found");

        var multiplexer = ConnectionMultiplexer.Connect(redisConnectionString);

        services.AddSingleton<IConnectionMultiplexer>(multiplexer);
        services.AddSingleton<ILoginAttemptTracker, RedisLoginAttemptTracker>();

        services.AddRateLimiter(options =>
        {
            options.AddPolicy(RateLimiterPolicies.Signup, httpContext =>
                RedisRateLimitPartition.GetFixedWindowRateLimiter(
                    httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    _ => new RedisFixedWindowRateLimiterOptions
                    {
                        PermitLimit = 3,
                        Window = TimeSpan.FromHours(1),
                        ConnectionMultiplexerFactory = () => multiplexer
                    }));

            options.AddPolicy(RateLimiterPolicies.UsernameCheck, httpContext =>
                RedisRateLimitPartition.GetFixedWindowRateLimiter(
                    httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    _ => new RedisFixedWindowRateLimiterOptions
                    {
                        PermitLimit = 20,
                        Window = TimeSpan.FromMinutes(1),
                        ConnectionMultiplexerFactory = () => multiplexer
                    }));

            options.AddPolicy(RateLimiterPolicies.EmailValidation, httpContext =>
                RedisRateLimitPartition.GetFixedWindowRateLimiter(
                    httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    _ => new RedisFixedWindowRateLimiterOptions
                    {
                        PermitLimit = 3,
                        Window = TimeSpan.FromMinutes(10),
                        ConnectionMultiplexerFactory = () => multiplexer
                    }));

            options.AddPolicy(RateLimiterPolicies.PasswordRecovery, httpContext =>
                RedisRateLimitPartition.GetFixedWindowRateLimiter(
                    httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    _ => new RedisFixedWindowRateLimiterOptions
                    {
                        PermitLimit = 3,
                        Window = TimeSpan.FromHours(1),
                        ConnectionMultiplexerFactory = () => multiplexer
                    }));

            options.RejectionStatusCode = 429;
        });
    }
}