using Capychef.Common.Auth;
using Capychef.Common.Errors;
using Capychef.Common.Result;
using Capychef.Users.Domain.DTO;

namespace Capychef.Households.Domain.Interfaces;

public interface IHouseholdMemberService
{
    Task<Result<List<UserDTO>>> GetHouseholdMembers(AuthUserDetails userDetails);
    Task<AppError?> LeaveHousehold(AuthUserDetails userDetails);
    Task<AppError> RemoveMember(AuthUserDetails userDetails, int householdMemberId);
}