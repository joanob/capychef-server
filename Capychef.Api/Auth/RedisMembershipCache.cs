using Capychef.Households.Domain.Interfaces;

namespace Capychef.Api.Auth;

public class RedisMembershipCache(IConnectionMultiplexer redis) : IMembershipCache
{
    private static readonly TimeSpan Ttl = TimeSpan.FromMinutes(15);

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

    public async Task<bool?> GetOwnershipAsync(int userId, int householdId)
    {
        var db = redis.GetDatabase();
        var value = await db.StringGetAsync(OwnershipKey(userId, householdId));
        if (value.IsNull) return null;
        return value == "1";
    }

    public async Task SetOwnershipAsync(int userId, int householdId, bool isOwner)
    {
        var db = redis.GetDatabase();
        await db.StringSetAsync(OwnershipKey(userId, householdId), isOwner ? "1" : "0", Ttl);
    }

    public async Task InvalidateOwnershipAsync(int userId, int householdId)
    {
        var db = redis.GetDatabase();
        await db.KeyDeleteAsync(OwnershipKey(userId, householdId));
    }

    private static string Key(int userId, int householdId)
    {
        return $"membership:{userId}:{householdId}";
    }

    private static string OwnershipKey(int userId, int householdId)
    {
        return $"ownership:{userId}:{householdId}";
    }
}