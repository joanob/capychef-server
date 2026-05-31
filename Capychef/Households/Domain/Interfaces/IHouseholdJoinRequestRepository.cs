using Capychef.Households.Domain.Entities;

namespace Capychef.Households.Domain.Interfaces;

public interface IHouseholdJoinRequestRepository
{
    Task AddJoinRequestAsync(HouseholdJoinRequest invitation);
    Task<List<HouseholdJoinRequest>> GetHouseholdJoinRequests(int householdId);
    Task<List<HouseholdJoinRequest>> GetHouseholdJoinRequestsByUserId(int userId);
    Task<HouseholdJoinRequest?> GetTrackedJoinRequestById(int invitationId, int householdId);
    Task<bool> CheckPendingJoinRequestExistsByHouseholdIdAndUserId(int householdId, int userId);
}