using Capychef.Api.Auth;
using Capychef.Api.Authorization;
using Capychef.Api.Errors;
using Capychef.Shopping.Domain.Cmd;
using Capychef.Shopping.Domain.DTO;
using Capychef.Shopping.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("shopping")]
public class ShoppingController(
    ISupermarketService supermarketService,
    IShoppingListItemService shoppingListItemService,
    ILoggerFactory loggerFactory
) : ControllerBase
{
    [CheckMembership]
    [HttpPost("supermarkets")]
    public async Task<ActionResult<SupermarketDto>> CreateSupermarket(SupermarketCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var supermarket = await supermarketService.CreateSupermarket(userDetails, cmd);

        var logger = loggerFactory.CreateLogger("SupermarketService.CreateSupermarket");

        if (supermarket.Failed()) return GlobalErrorHandler.HandleError(supermarket.Error(), logger);

        return Ok(supermarket.Get());
    }

    [CheckMembership]
    [HttpPut("supermarkets/{id}")]
    public async Task<ActionResult> UpdateSupermarket(int id, SupermarketCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var error = await supermarketService.UpdateSupermarket(userDetails, id, cmd);

        var logger = loggerFactory.CreateLogger("SupermarketService.UpdateSupermarket");

        if (error != null) return GlobalErrorHandler.HandleError(error, logger);

        return Ok();
    }

    [CheckMembership]
    [HttpDelete("supermarkets/{id}")]
    public async Task<ActionResult> DeleteSupermarket(int id)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var error = await supermarketService.DeleteSupermarket(userDetails, id);

        var logger = loggerFactory.CreateLogger("SupermarketService.DeleteSupermarket");

        if (error != null) return GlobalErrorHandler.HandleError(error, logger);

        return Ok();
    }

    [CheckMembership]
    [HttpPost("list")]
    public async Task<ActionResult<ShoppingListItemDto>> CreateShoppingListItem(ShoppingListItemCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var result = await shoppingListItemService.CreateShoppingListItem(userDetails, cmd);

        var logger = loggerFactory.CreateLogger("ShoppingListItemService.CreateShoppingListItem");

        if (result.Failed()) return GlobalErrorHandler.HandleError(result.Error(), logger);

        return Ok(result.Get());
    }

    [CheckMembership]
    [HttpPut("list/{id}")]
    public async Task<ActionResult<ShoppingListItemDto>> UpdateShoppingListItem(int id, ShoppingListItemCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var result = await shoppingListItemService.UpdateShoppingListItem(userDetails, id, cmd);

        var logger = loggerFactory.CreateLogger("ShoppingListItemService.UpdateShoppingListItem");

        if (result.Failed()) return GlobalErrorHandler.HandleError(result.Error(), logger);

        return Ok(result.Get());
    }

    [CheckMembership]
    [HttpDelete("list/{id}")]
    public async Task<ActionResult> DeleteShoppingListItem(int id)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var error = await shoppingListItemService.DeleteShoppingListItem(userDetails, id);

        var logger = loggerFactory.CreateLogger("ShoppingListItemService.DeleteShoppingListItem");

        if (error != null) return GlobalErrorHandler.HandleError(error, logger);

        return Ok();
    }
}