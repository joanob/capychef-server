using Capychef.Households.Domain.Entities;

namespace Capychef.Households.Domain.Interfaces;

public interface IHouseholdInvitationRepository
{
    Task AddInvitationAsync(HouseholdInvitation invitation);
    Task<List<HouseholdInvitation>> GetHouseholdInvitations(int householdId);
    Task<List<HouseholdInvitation>> GetAllHouseholdInvitationsHistory(int householdId);
    Task<List<HouseholdInvitation>> GetHouseholdInvitationsByUserId(int userId);
    Task<HouseholdInvitation?> GetTrackedInvitationById(int invitationId, int userId);
    Task<HouseholdInvitation?> GetTrackedInvitationByIdAndHouseholdId(int invitationId, int householdId);
    Task<bool> CheckNonAnsweredInvitationExistsByHouseholdIdAndUserId(int householdId, int userId);
}