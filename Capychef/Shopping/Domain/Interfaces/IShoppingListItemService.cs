using Capychef.Common.Auth;
using Capychef.Common.Errors;
using Capychef.Common.Result;
using Capychef.Shopping.Domain.Cmd;
using Capychef.Shopping.Domain.DTO;

namespace Capychef.Shopping.Domain.Interfaces;

public interface IShoppingListItemService
{
    Task<Result<ShoppingListItemDto>> CreateShoppingListItem(AuthUserDetails userDetails, ShoppingListItemCmd cmd);

    Task<Result<ShoppingListItemDto>> UpdateShoppingListItem(AuthUserDetails userDetails, int id,
        ShoppingListItemCmd cmd);

    Task<AppError?> DeleteShoppingListItem(AuthUserDetails userDetails, int id);

    Task<AppError?> MarkAsPurchased(AuthUserDetails userDetails, int id);

    Task<AppError?> MarkAsNotPurchased(AuthUserDetails userDetails, int id);

    Task<AppError?> StoreShoppingListItem(AuthUserDetails userDetails, int id, StoreShoppingListItemCmd cmd);
}