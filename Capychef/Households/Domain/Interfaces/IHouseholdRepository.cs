using Capychef.Households.Domain.Entities;

namespace Capychef.Households.Domain.Interfaces;

public interface IHouseholdRepository
{
    Task AddHouseholdAsync(Household household);
    Task<Household?> GetHouseholdByPublicIdAsync(string publicId);
    Task<bool> CheckHouseholdOwnership(int userId, int householdId);
    Task<List<Household>> GetAllHouseholds(int userId);
    Task<Household?> GetHouseholdById(int householdId);
    Task<Household?> GetTrackedHouseholdById(int householdId);
}