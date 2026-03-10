using Capychef.Common.Errors;

namespace Capychef.Gourmet.Domain.Interfaces;

public interface ISubscriptionService
{
    Task<AppError?> CheckUserCanCreateHousehold(int userId);
    Task<AppError?> CheckUserCanBecomeHouseholdMember(int userId);
    Task<bool> WillReachMembershipLimit(int userId, int additionalMemberships);
}