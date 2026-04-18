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

public class ShoppingListItemService(
    CapychefDbContext dbContext,
    IShoppingListItemRepository shoppingListItemRepository)
    : IShoppingListItemService
{
    public async Task<Result<ShoppingListItemDto>> CreateShoppingListItem(AuthUserDetails userDetails,
        ShoppingListItemCmd cmd)
    {
        var validationError = cmd.Validate();
        if (validationError != null) return new Result<ShoppingListItemDto>(validationError);

        int? preferredSupermarketId = null;

        if (cmd.FoodId.HasValue)
            preferredSupermarketId =
                await shoppingListItemRepository.GetPreferredSupermarketId(cmd.FoodId.Value, cmd.FoodUoMId,
                    userDetails.GetHouseholdId());

        var item = new ShoppingListItem(
            cmd.FoodId,
            cmd.Name,
            cmd.Quantity,
            cmd.FoodUoMId,
            preferredSupermarketId,
            userDetails.GetHouseholdId(),
            userDetails.UserId
        );

        await shoppingListItemRepository.AddAsync(item);
        await dbContext.SaveChangesAsync();

        return new Result<ShoppingListItemDto>(new ShoppingListItemDto(item));
    }

    public async Task<Result<ShoppingListItemDto>> UpdateShoppingListItem(AuthUserDetails userDetails, int id,
        ShoppingListItemCmd cmd)
    {
        var validationError = cmd.Validate();
        if (validationError != null) return new Result<ShoppingListItemDto>(validationError);

        var item = await shoppingListItemRepository.FindTrackedById(id, userDetails.GetHouseholdId());
        if (item == null)
            return new Result<ShoppingListItemDto>(new NotFoundError(EntityType.ShoppingListItem, id));

        var preferredSupermarketId = item.PreferredSupermarketId;

        if (cmd.FoodId.HasValue)
            preferredSupermarketId =
                await shoppingListItemRepository.GetPreferredSupermarketId(cmd.FoodId.Value, cmd.FoodUoMId,
                    userDetails.GetHouseholdId());

        item.Update(
            cmd.FoodId,
            cmd.Name,
            cmd.Quantity,
            cmd.FoodUoMId,
            preferredSupermarketId
        );

        await shoppingListItemRepository.UpdateAsync(item);
        await dbContext.SaveChangesAsync();

        return new Result<ShoppingListItemDto>(new ShoppingListItemDto(item));
    }

    public async Task<AppError?> DeleteShoppingListItem(AuthUserDetails userDetails, int id)
    {
        var item = await shoppingListItemRepository.FindTrackedById(id, userDetails.GetHouseholdId());
        if (item == null)
            return new NotFoundError(EntityType.ShoppingListItem, id);

        item.Delete();
        await dbContext.SaveChangesAsync();

        return null;
    }

    public async Task<AppError?> MarkAsPurchased(AuthUserDetails userDetails, int id)
    {
        var item = await shoppingListItemRepository.FindTrackedById(id, userDetails.GetHouseholdId());
        if (item == null)
            return new NotFoundError(EntityType.ShoppingListItem, id);

        item.Purchase(userDetails.UserId);
        await dbContext.SaveChangesAsync();

        return null;
    }

    public async Task<AppError?> MarkAsNotPurchased(AuthUserDetails userDetails, int id)
    {
        var item = await shoppingListItemRepository.FindTrackedById(id, userDetails.GetHouseholdId());
        if (item == null)
            return new NotFoundError(EntityType.ShoppingListItem, id);

        item.Unpurchase();
        await dbContext.SaveChangesAsync();

        return null;
    }
}