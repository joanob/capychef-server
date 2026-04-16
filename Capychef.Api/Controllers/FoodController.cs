using Capychef.Api.Auth;
using Capychef.Api.Authorization;
using Capychef.Api.Errors;
using Capychef.Food.Domain.Cmd;
using Capychef.Food.Domain.DTO;
using Capychef.Food.Domain.Interfaces;
using Capychef.Shopping.Domain.Cmd;
using Capychef.Shopping.Domain.DTO;
using Capychef.Shopping.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("food")]
public class FoodController(
    IFoodService foodService,
    ISupermarketFoodDetailsService supermarketFoodDetailsService,
    ILoggerFactory loggerFactory) : ControllerBase
{
    [CheckMembership]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<FoodDto>>> CreateHouseholdFood(HouseholdFoodCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var food = await foodService.CreateHouseholdFood(userDetails, cmd);

        var logger = loggerFactory.CreateLogger("FoodService.CreateHousehold");

        if (food.Failed()) return GlobalErrorHandler.HandleError(food.Error(), logger);

        return Ok(new ApiResponse<FoodDto>(food.Get()));
    }

    [CheckMembership]
    [HttpGet("household/{householdId}")]
    public async Task<ActionResult<ApiResponse<List<FoodDto>>>> GetAllHouseholdFood(int householdId)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var foodList = await foodService.GetAllHouseholdFood(userDetails);

        return Ok(new ApiResponse<List<FoodDto>>(foodList));
    }

    [CheckMembership]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<FoodDto>>> GetFoodById(int id)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var food = await foodService.GetFoodById(userDetails, id);

        var logger = loggerFactory.CreateLogger("FoodService.GetFoodById");

        if (food.Failed()) return GlobalErrorHandler.HandleError(food.Error(), logger);

        return Ok(new ApiResponse<FoodDto>(food.Get()));
    }

    [CheckMembership]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<FoodDto>>> UpdateFood(int id, HouseholdFoodCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var food = await foodService.UpdateFood(id, cmd, userDetails);

        var logger = loggerFactory.CreateLogger("FoodService.UpdateHouseholdFood");

        if (food.Failed()) return GlobalErrorHandler.HandleError(food.Error(), logger);

        return Ok(new ApiResponse<FoodDto>(food.Get()));
    }

    [CheckMembership]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteHouseholdFood(int id)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var error = await foodService.DeleteHouseholdFood(id, userDetails);

        var logger = loggerFactory.CreateLogger("FoodService.DeleteHouseholdFood");

        if (error != null) return GlobalErrorHandler.HandleError(error, logger);

        return Ok();
    }

    [CheckMembership]
    [HttpPost("{foodId}/supermarket-details")]
    public async Task<ActionResult<ApiResponse<SupermarketFoodDetailsDto>>> CreateSupermarketFoodDetails(
        int foodId, SupermarketFoodDetailsCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var details = await supermarketFoodDetailsService.CreateSupermarketFoodDetails(userDetails, foodId, cmd);

        var logger = loggerFactory.CreateLogger("SupermarketFoodDetailsService.CreateSupermarketFoodDetails");

        if (details.Failed()) return GlobalErrorHandler.HandleError(details.Error(), logger);

        return Ok(new ApiResponse<SupermarketFoodDetailsDto>(details.Get()));
    }

    [CheckMembership]
    [HttpPut("{foodId}/supermarket-details/{id}")]
    public async Task<ActionResult> UpdateSupermarketFoodDetails(
        int foodId, int id, SupermarketFoodDetailsCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var error = await supermarketFoodDetailsService.UpdateSupermarketFoodDetails(userDetails, foodId, id, cmd);

        var logger = loggerFactory.CreateLogger("SupermarketFoodDetailsService.UpdateSupermarketFoodDetails");

        if (error != null) return GlobalErrorHandler.HandleError(error, logger);

        return Ok();
    }

    [CheckMembership]
    [HttpDelete("{foodId}/supermarket-details/{id}")]
    public async Task<ActionResult> DeleteSupermarketFoodDetails(int foodId, int id)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var error = await supermarketFoodDetailsService.DeleteSupermarketFoodDetails(userDetails, foodId, id);

        var logger = loggerFactory.CreateLogger("SupermarketFoodDetailsService.DeleteSupermarketFoodDetails");

        if (error != null) return GlobalErrorHandler.HandleError(error, logger);

        return Ok();
    }
}