using Capychef.Common.Auth;
using Capychef.Common.Entities;
using Capychef.Common.Errors;
using Capychef.Common.Result;
using Capychef.Persistence;
using Capychef.Shopping.Domain.Cmd;
using Capychef.Shopping.Domain.DTO;
using Capychef.Shopping.Domain.Entities;
using Capychef.Shopping.Domain.Interfaces;

namespace Capychef.Shopping.Services;

public class SupermarketService(CapychefDbContext dbContext, ISupermarketRepository supermarketRepository)
    : ISupermarketService
{
    public async Task<Result<SupermarketDto>> CreateSupermarket(AuthUserDetails userDetails, SupermarketCmd cmd)
    {
        var validationError = cmd.Validate();
        if (validationError != null) return new Result<SupermarketDto>(validationError);

        var supermarket = new Supermarket(cmd.Name, userDetails.GetHouseholdId(), userDetails.UserId);

        await supermarketRepository.AddAsync(supermarket);
        await dbContext.SaveChangesAsync();

        return new Result<SupermarketDto>(new SupermarketDto(supermarket));
    }

    public async Task<AppError?> UpdateSupermarket(AuthUserDetails userDetails, int id, SupermarketCmd cmd)
    {
        var validationError = cmd.Validate();
        if (validationError != null) return validationError;

        var supermarket = await supermarketRepository.FindTrackedById(id);
        if (supermarket == null || supermarket.HouseholdId != userDetails.GetHouseholdId())
            return new NotFoundError(EntityType.Supermarket, id);

        supermarket.Name = cmd.Name;

        await dbContext.SaveChangesAsync();

        return null;
    }

    public async Task<AppError?> DeleteSupermarket(AuthUserDetails userDetails, int id)
    {
        var supermarket = await supermarketRepository.FindTrackedById(id);
        if (supermarket == null || supermarket.HouseholdId != userDetails.GetHouseholdId())
            return new NotFoundError(EntityType.Supermarket, id);

        supermarket.Delete();

        await dbContext.SaveChangesAsync();

        return null;
    }
}