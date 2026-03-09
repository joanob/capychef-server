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
    ISubscriptionService subscriptionService
) : IHouseholdService
{
    public async Task<Result<HouseholdDTO>> CreateHousehold(AuthUserDetails userDetails, HouseholdCmd cmd)
    {
        var error = await subscriptionService.CheckUserCanCreateHousehold(userDetails.UserId);

        if (error != null) return new Result<HouseholdDTO>(error);

        var household = new Household(userDetails.UserId, cmd.Name);

        await householdRepository.AddHouseholdAsync(household);

        await createDefaultStorageSpaces(household, userDetails);

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
        var household = await householdRepository.GetHouseholdByIdIncludingStorageSpaces(userDetails.HouseholdId.Value);
        if (household == null)
            return new Result<HouseholdDTO>(new NotFoundError(EntityType.Household, userDetails.HouseholdId.Value));

        return new Result<HouseholdDTO>(new HouseholdDTO(household));
    }

    private async Task createDefaultStorageSpaces(Household household, AuthUserDetails userDetails)
    {
        var storageSpace = new StorageSpace("Despensa", StorageConditions.AmbientTemperature,
            household, userDetails.UserId);
        household.AddStorageSpace(storageSpace);
        await storageSpaceRepository.AddAsync(storageSpace);

        storageSpace = new StorageSpace("Nevera", StorageConditions.Refrigerated,
            household, userDetails.UserId);
        household.AddStorageSpace(storageSpace);
        await storageSpaceRepository.AddAsync(storageSpace);

        storageSpace = new StorageSpace("Congelador", StorageConditions.Frozen,
            household, userDetails.UserId);
        household.AddStorageSpace(storageSpace);
        await storageSpaceRepository.AddAsync(storageSpace);
    }
}