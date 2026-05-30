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
    ISubscriptionService subscriptionService,
    IMembershipCache membershipCache
) : IHouseholdService
{
    public async Task<Result<HouseholdDto>> CreateHousehold(AuthUserDetails userDetails, CreateHouseholdCmd cmd)
    {
        var error = await subscriptionService.CheckUserCanCreateHousehold(userDetails.UserId);

        if (error != null) return new Result<HouseholdDto>(error);

        var validationError = cmd.Validate();

        if (validationError != null) return new Result<HouseholdDto>(validationError);

        var household = new Household(userDetails.UserId, cmd.Name);

        await householdRepository.AddHouseholdAsync(household);

        await CreateInitialStorageSpaces(household, userDetails, cmd);

        var member = new HouseholdMember(household, userDetails.UserId);

        await householdMemberRepository.AddHouseholdMemberAsync(member);

        await dbContext.SaveChangesAsync();

        return new Result<HouseholdDto>(new HouseholdDto(household));
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

    public async Task<List<HouseholdDto>> GetAllHouseholds(AuthUserDetails userDetails)
    {
        var households = await householdRepository.GetAllHouseholds(userDetails.UserId);

        return HouseholdDto.ToDtoList(households);
    }

    public async Task<Result<HouseholdDto>> GetHouseholdById(AuthUserDetails userDetails, int householdId)
    {
        var household = await householdRepository.GetHouseholdById(householdId);

        if (household == null) return new Result<HouseholdDto>(new NotFoundError(EntityType.Household, householdId));

        if (!await householdMemberRepository.CheckHouseholdMembership(userDetails.UserId, householdId))
            return new Result<HouseholdDto>(new HouseholdMembershipError(userDetails.UserId, householdId));

        return new Result<HouseholdDto>(new HouseholdDto(household));
    }

    public async Task<Result<HouseholdDto>> SelectHousehold(AuthUserDetails userDetails, int householdId)
    {
        if (!await householdMemberRepository.CheckHouseholdMembership(userDetails.UserId, householdId))
            return new Result<HouseholdDto>(new HouseholdMembershipError(userDetails.UserId, householdId));

        var household = await householdRepository.GetHouseholdById(householdId);
        if (household == null) return new Result<HouseholdDto>(new NotFoundError(EntityType.Household, householdId));

        return new Result<HouseholdDto>(new HouseholdDto(household));
    }

    public async Task<Result<HouseholdDto>> UpdateHousehold(AuthUserDetails userDetails, int householdId,
        HouseholdCmd cmd)
    {
        if (householdId != userDetails.GetHouseholdId())
            return new Result<HouseholdDto>(new ValidationError("HouseholdId does not match the active household"));

        var validationError = cmd.Validate();
        if (validationError != null) return new Result<HouseholdDto>(validationError);

        var household = await householdRepository.GetTrackedHouseholdById(householdId);

        if (household == null) return new Result<HouseholdDto>(new NotFoundError(EntityType.Household, householdId));

        household.Name = cmd.Name;

        await dbContext.SaveChangesAsync();

        return new Result<HouseholdDto>(new HouseholdDto(household));
    }

    public async Task<Result<HouseholdDto>> GetActiveHousehold(AuthUserDetails userDetails)
    {
        var household = await householdRepository.GetHouseholdByIdIncludingStorageSpaces(userDetails.GetHouseholdId());
        if (household == null)
            return new Result<HouseholdDto>(new NotFoundError(EntityType.Household, userDetails.GetHouseholdId()));

        return new Result<HouseholdDto>(new HouseholdDto(household));
    }

    public async Task<AppError?> DeleteHousehold(AuthUserDetails userDetails, int householdId)
    {
        if (householdId != userDetails.GetHouseholdId())
            return new ValidationError("HouseholdId does not match the active household");

        var household = await householdRepository.GetTrackedHouseholdById(userDetails.GetHouseholdId());
        if (household == null) return new NotFoundError(EntityType.Household, userDetails.GetHouseholdId());

        household.Delete();

        await dbContext.SaveChangesAsync();

        await membershipCache.InvalidateOwnershipAsync(userDetails.UserId, userDetails.GetHouseholdId());

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

            await storageSpaceRepository.AddAsync(s);
        }
    }
}