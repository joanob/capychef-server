using Capychef.Common.Auth;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;
using Capychef.Households.Domain.Entities;
using Capychef.Households.Domain.Interfaces;
using Capychef.Persistence;
using Capychef.Users.Domain.Errors;
using YourOwnBoss.Common.Errors;
using YourOwnBoss.Common.Result;

namespace Capychef.Households.Services;

public class HouseholdService(
    CapychefDbContext dbContext,
    IHouseholdRepository householdRepository,
    IHouseholdMemberRepository householdMemberRepository
) : IHouseholdService
{
    public async Task<Result<HouseholdDTO>> CreateHousehold(AuthUserDetails userDetails, CreateHouseholdCmd cmd)
    {
        var household = new Household(userDetails.UserId, cmd.Name);

        await householdRepository.AddHouseholdAsync(household);

        var member = new HouseholdMember(household, userDetails.UserId);

        await householdMemberRepository.AddHouseholdMemberAsync(member);

        await dbContext.SaveChangesAsync();

        return new Result<HouseholdDTO>(new HouseholdDTO(household));
    }

    public async Task<AppError?> CheckHouseholdOwnership(AuthUserDetails userDetails)
    {
        if (userDetails.HouseholdId == null) return new NoHouseholdSelectedError(userDetails.UserId);

        if (await householdRepository.CheckHouseholdOwnership(userDetails.UserId, userDetails.HouseholdId.Value))
            return null;

        return new HouseholdMembershipError(userDetails.UserId, userDetails.HouseholdId.Value);
    }

    public async Task<AppError?> CheckHouseholdMembership(AuthUserDetails userDetails)
    {
        if (userDetails.HouseholdId == null) return new NoHouseholdSelectedError(userDetails.UserId);

        if (await householdMemberRepository.CheckHouseholdMembership(userDetails.UserId, userDetails.HouseholdId.Value))
            return null;

        return new HouseholdMembershipError(userDetails.UserId, userDetails.HouseholdId.Value);
    }
}