using Capychef.Households.Domain.Interfaces;
using StackExchange.Redis;

namespace Capychef.Api.Auth;

public class RedisMembershipCache(IConnectionMultiplexer redis) : IMembershipCache
{
    private static readonly TimeSpan Ttl = TimeSpan.FromMinutes(15);

    private static string Key(int userId, int householdId) => $"membership:{userId}:{householdId}";

    public async Task<bool?> GetAsync(int userId, int householdId)
    {
        var db = redis.GetDatabase();
        var value = await db.StringGetAsync(Key(userId, householdId));
        if (value.IsNull) return null;
        return value == "1";
    }

    public async Task SetAsync(int userId, int householdId, bool isMember)
    {
        var db = redis.GetDatabase();
        await db.StringSetAsync(Key(userId, householdId), isMember ? "1" : "0", Ttl);
    }

    public async Task InvalidateAsync(int userId, int householdId)
    {
        var db = redis.GetDatabase();
        await db.KeyDeleteAsync(Key(userId, householdId));
    }
}

