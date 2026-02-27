namespace Capychef.Api.Realtime;

public interface IConnectionMappingStore
{
    Task AddAsync(ConnectionDetails connection);
    Task RemoveAsync(string connectionId);
    Task<IEnumerable<ConnectionDetails>> GetByUserAsync(int userId);
    Task<IEnumerable<ConnectionDetails>> GetByHouseholdAsync(int householdId);
    Task<IEnumerable<ConnectionDetails>> GetByUserAndHouseholdAsync(int userId, int householdId);
    Task UpdateHouseholdAsync(string connectionId, int? householdId);
}