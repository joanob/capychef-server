using Capychef.Common.Auth;
using Capychef.Common.Entities;
using Capychef.Common.Errors;
using Capychef.Common.Result;
using Capychef.Households.Domain.Interfaces;
using Capychef.Persistence;
using Capychef.Users.Domain.DTO;

namespace Capychef.Households.Services;

public class HouseholdMemberService(
    CapychefDbContext dbContext,
    IHouseholdMemberRepository householdMemberRepository
) : IHouseholdMemberService
{
    public async Task<Result<List<UserDTO>>> GetHouseholdMembers(AuthUserDetails userDetails)
    {
        var members = await householdMemberRepository.GetHouseholdMembers(userDetails.HouseholdId.Value);

        return new Result<List<UserDTO>>(UserDTO.ToDTOList(members.Select(m => m.User).ToList()));
    }

    public async Task<AppError?> LeaveHousehold(AuthUserDetails userDetails)
    {
        var member =
            await householdMemberRepository.GetTrackedHouseholdMember(userDetails.UserId,
                userDetails.HouseholdId.Value);

        if (member == null)
            return new NotFoundError(EntityType.HouseholdMember,
                $"household {userDetails.HouseholdId.Value} user {userDetails.UserId}");

        member.Leave();

        await dbContext.SaveChangesAsync();

        return null;
    }
}