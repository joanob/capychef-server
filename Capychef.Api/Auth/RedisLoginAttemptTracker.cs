namespace Capychef.Api.Auth;

public class RedisLoginAttemptTracker(IConnectionMultiplexer redis) : ILoginAttemptTracker
{
    private const int MaxFailedAttempts = 5;
    private static readonly TimeSpan Window = TimeSpan.FromHours(1);

    public async Task<bool> IsBlockedAsync(string ip)
    {
        var db = redis.GetDatabase();
        var count = (int?)await db.StringGetAsync(Key(ip));
        return count >= MaxFailedAttempts;
    }

    public async Task RecordFailedAttemptAsync(string ip)
    {
        var db = redis.GetDatabase();
        var key = Key(ip);
        var count = await db.StringIncrementAsync(key);
        if (count == 1)
            await db.KeyExpireAsync(key, Window);
    }

    public async Task ResetAsync(string ip)
    {
        var db = redis.GetDatabase();
        await db.KeyDeleteAsync(Key(ip));
    }

    private static string Key(string ip)
    {
        return $"login_attempts:{ip}";
    }
}