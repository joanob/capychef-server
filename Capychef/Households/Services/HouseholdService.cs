using Capychef.Common.Auth;
using Capychef.Common.Entities;
using Capychef.Common.Errors;
using Capychef.Common.Result;
using Capychef.Gourmet.Domain.Interfaces;
using Capychef.Households.Domain.Cmd;
using Capychef.Households.Domain.DTO;
using Capychef.Households.Domain.Entities;
using Capychef.Households.Domain.Errors;
using Capychef.Households.Domain.Interfaces;
using Capychef.Persistence;

namespace Capychef.Households.Services;

public class HouseholdService(
    CapychefDbContext dbContext,
    IHouseholdRepository householdRepository,
    IHouseholdMemberRepository householdMemberRepository,
    IStorageSpaceRepository storageSpaceRepository,
    IHouseholdJoinRequestRepository joinRequestRepository,
    ISubscriptionService subscriptionService
) : IHouseholdService
{
    public async Task<Result<HouseholdDTO>> CreateHousehold(AuthUserDetails userDetails, CreateHouseholdCmd cmd)
    {
        var error = await subscriptionService.CheckUserCanCreateHousehold(userDetails.UserId);

        if (error != null) return new Result<HouseholdDTO>(error);

        var household = new Household(userDetails.UserId, cmd.Name);

        await householdRepository.AddHouseholdAsync(household);

        await CreateInitialStorageSpaces(household, userDetails, cmd);

        var member = new HouseholdMember(household, userDetails.UserId);

        await householdMemberRepository.AddHouseholdMemberAsync(member);

        if (await subscriptionService.WillReachMembershipLimit(userDetails.UserId, 1))
            await joinRequestRepository.HidePendingJoinRequestsForUser(userDetails.UserId);

        await dbContext.SaveChangesAsync();

        return new Result<HouseholdDTO>(new HouseholdDTO(household));
    }

    public async Task<AppError?> CheckHouseholdOwnership(AuthUserDetails userDetails)
    {
        if (!userDetails.HasHouseholdId) return new NoHouseholdSelectedError(userDetails.UserId);

        if (await householdRepository.CheckHouseholdOwnership(userDetails.UserId, userDetails.GetHouseholdId()))
            return null;

        return new HouseholdMembershipError(userDetails.UserId, userDetails.GetHouseholdId());
    }

    public async Task<AppError?> CheckHouseholdMembership(AuthUserDetails userDetails)
    {
        if (!userDetails.HasHouseholdId) return new NoHouseholdSelectedError(userDetails.UserId);

        if (await householdMemberRepository.CheckHouseholdMembership(userDetails.UserId, userDetails.GetHouseholdId()))
            return null;

        return new HouseholdMembershipError(userDetails.UserId, userDetails.GetHouseholdId());
    }

    public async Task<List<HouseholdDTO>> GetAllHouseholds(AuthUserDetails userDetails)
    {
        var households = await householdRepository.GetAllHouseholds(userDetails.UserId);

        return HouseholdDTO.ToDTOList(households);
    }

    public async Task<Result<HouseholdDTO>> GetHouseholdById(AuthUserDetails userDetails, int householdId)
    {
        var household = await householdRepository.GetHouseholdById(householdId);

        if (household == null) return new Result<HouseholdDTO>(new NotFoundError(EntityType.Household, householdId));

        if (!await householdMemberRepository.CheckHouseholdMembership(userDetails.UserId, householdId))
            return new Result<HouseholdDTO>(new HouseholdMembershipError(userDetails.UserId, householdId));

        return new Result<HouseholdDTO>(new HouseholdDTO(household));
    }

    public async Task<Result<HouseholdDTO>> SelectHousehold(AuthUserDetails userDetails, int householdId)
    {
        if (!await householdMemberRepository.CheckHouseholdMembership(userDetails.UserId, householdId))
            return new Result<HouseholdDTO>(new HouseholdMembershipError(userDetails.UserId, householdId));

        var household = await householdRepository.GetHouseholdById(householdId);
        if (household == null) return new Result<HouseholdDTO>(new NotFoundError(EntityType.Household, householdId));

        return new Result<HouseholdDTO>(new HouseholdDTO(household));
    }

    public async Task<Result<HouseholdDTO>> UpdateHousehold(AuthUserDetails userDetails, int householdId,
        HouseholdCmd cmd)
    {
        var household = await householdRepository.GetTrackedHouseholdById(householdId);

        household.Name = cmd.Name;

        await dbContext.SaveChangesAsync();

        return new Result<HouseholdDTO>(new HouseholdDTO(household));
    }

    public async Task<Result<HouseholdDTO>> GetActiveHousehold(AuthUserDetails userDetails)
    {
        var household = await householdRepository.GetHouseholdByIdIncludingStorageSpaces(userDetails.GetHouseholdId());
        if (household == null)
            return new Result<HouseholdDTO>(new NotFoundError(EntityType.Household, userDetails.GetHouseholdId()));

        return new Result<HouseholdDTO>(new HouseholdDTO(household));
    }

    public async Task<AppError> DeleteHousehold(AuthUserDetails userDetails)
    {
        var household = await householdRepository.GetTrackedHouseholdById(userDetails.GetHouseholdId());
        if (household == null) return new NotFoundError(EntityType.Household, userDetails.GetHouseholdId());

        household.Delete();

        await dbContext.SaveChangesAsync();

        return null;
    }

    private async Task CreateInitialStorageSpaces(Household household, AuthUserDetails userDetails,
        CreateHouseholdCmd cmd)
    {
        if (cmd.InitialStorageSpaceIds.Count == 0)
            return;

        var selected = await storageSpaceRepository.GetInitialStorageSpacesByIds(cmd.InitialStorageSpaceIds);
        foreach (var initial in selected)
        {
            var s = new StorageSpace(initial.Name, initial.StorageCondition, household, userDetails.UserId);
            household.AddStorageSpace(s);
            await storageSpaceRepository.AddAsync(s);
        }
    }
}