using Capychef.Households.Domain.Entities;

namespace Capychef.Households.Domain.Interfaces;

public interface IHouseholdMemberRepository
{
    Task AddHouseholdMemberAsync(HouseholdMember member);
    Task<bool> CheckHouseholdMembership(int userId, int householdId);
    Task<List<HouseholdMember>> GetHouseholdMembers(int householdId);
    Task<int> CountHouseholdMemberships(int userId);
    Task<Dictionary<int, int>> CountMembersByHouseholdIds(List<int> householdIds);
    Task<HouseholdMember?> GetTrackedHouseholdMember(int userId, int householdId);
    Task<HouseholdMember?> GetTrackedByHouseholdMemberId(int householdMemberId, int householdId);
}