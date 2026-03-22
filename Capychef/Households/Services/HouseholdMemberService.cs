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
        var members = await householdMemberRepository.GetHouseholdMembers(userDetails.GetHouseholdId());

        return new Result<List<UserDTO>>(UserDTO.ToDTOList(members.Select(m => m.User).ToList()));
    }

    public async Task<AppError?> LeaveHousehold(AuthUserDetails userDetails)
    {
        var member =
            await householdMemberRepository.GetTrackedHouseholdMember(userDetails.UserId,
                userDetails.GetHouseholdId());

        if (member == null)
            return new NotFoundError(EntityType.HouseholdMember,
                $"household {userDetails.GetHouseholdId()} user {userDetails.UserId}");

        member.Leave();

        await dbContext.SaveChangesAsync();

        return null;
    }

    public async Task<AppError> RemoveMember(AuthUserDetails userDetails, int householdMemberId)
    {
        var member =
            await householdMemberRepository.GetTrackedByHouseholdMemberId(householdMemberId,
                userDetails.GetHouseholdId());

        if (member == null) return new NotFoundError(EntityType.HouseholdMember, householdMemberId);

        member.Delete();

        await dbContext.SaveChangesAsync();

        return null;
    }
}