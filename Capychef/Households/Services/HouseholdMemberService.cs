using Capychef.Common.Auth;
using Capychef.Common.Entities;
using Capychef.Common.Errors;
using Capychef.Common.Result;
using Capychef.Households.Domain.Interfaces;
using Capychef.Persistence;
using Capychef.Users.Domain.DTO;
using Capychef.Users.Domain.Entities;

namespace Capychef.Households.Services;

public class HouseholdMemberService(
    CapychefDbContext dbContext,
    IHouseholdMemberRepository householdMemberRepository,
    IMembershipCache membershipCache
) : IHouseholdMemberService
{
    public async Task<Result<List<UserDto>>> GetHouseholdMembers(AuthUserDetails userDetails)
    {
        var members = await householdMemberRepository.GetHouseholdMembers(userDetails.GetHouseholdId());

        var users = new List<User>();

        foreach (var member in members)
            if (member.User != null)
                users.Add(member.User);

        return new Result<List<UserDto>>(UserDto.ToDtoList(users));
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

        await membershipCache.InvalidateAsync(userDetails.UserId, userDetails.GetHouseholdId());

        return null;
    }

    public async Task<AppError?> RemoveMember(AuthUserDetails userDetails, int householdMemberId)
    {
        var member =
            await householdMemberRepository.GetTrackedByHouseholdMemberId(householdMemberId,
                userDetails.GetHouseholdId());

        if (member == null) return new NotFoundError(EntityType.HouseholdMember, householdMemberId);

        member.Delete();

        await dbContext.SaveChangesAsync();

        await membershipCache.InvalidateAsync(member.UserId, member.HouseholdId);

        return null;
    }
}