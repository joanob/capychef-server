using Capychef.Api.Auth;
using Capychef.Api.Authorization;
using Capychef.Api.Errors;
using Capychef.Food.Domain.Cmd;
using Capychef.Food.Domain.DTO;
using Capychef.Food.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("food")]
public class FoodController(
    IServiceScopeFactory serviceScopeFactory,
    IFoodService foodService,
    ILoggerFactory loggerFactory) : ControllerBase
{
    [CheckMembership]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<FoodDTO>>> CreateHouseholdFood(CreateHouseholdFoodCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var food = await foodService.CreateHouseholdFood(userDetails, cmd);

        var logger = loggerFactory.CreateLogger("FoodService.CreateHousehold");

        if (food.failed()) return GlobalErrorHandler.handleError(food.error(), logger);

        return Ok(new ApiResponse<FoodDTO>(food.get()));
    }

    [HttpPost("global")]
    public async Task<ActionResult> LoadGlobalFood(GlobalFoodFileCmd fileCmd)
    {
        Task.Run(async () =>
        {
            using var scope = serviceScopeFactory.CreateScope();

            var foodService = scope.ServiceProvider.GetRequiredService<IFoodService>();

            try
            {
                await foodService.LoadGlobalFood(fileCmd);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        });

        return Ok();
    }

    [CheckMembership]
    [HttpGet("household/{householdId}")]
    public async Task<ActionResult<ApiResponse<List<FoodDTO>>>> GetAllHouseholdFood(int householdId)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var foodList = await foodService.GetAllHouseholdFood(userDetails);

        return Ok(new ApiResponse<List<FoodDTO>>(foodList));
    }

    [CheckMembership]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<FoodDTO>>> GetFoodById(int id)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var food = await foodService.GetFoodById(userDetails, id);

        var logger = loggerFactory.CreateLogger("FoodService.GetFoodById");

        if (food.failed()) return GlobalErrorHandler.handleError(food.error(), logger);

        return Ok(new ApiResponse<FoodDTO>(food.get()));
    }

    [CheckMembership]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<FoodDTO>>> UpdateHouseholdFood(int id, UpdateHouseholdFoodCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var food = await foodService.UpdateHouseholdFood(id, cmd, userDetails);

        var logger = loggerFactory.CreateLogger("FoodService.UpdateHouseholdFood");

        if (food.failed()) return GlobalErrorHandler.handleError(food.error(), logger);

        return Ok(new ApiResponse<FoodDTO>(food.get()));
    }

    [CheckMembership]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteHouseholdFood(int id)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var error = await foodService.DeleteHouseholdFood(id, userDetails);

        var logger = loggerFactory.CreateLogger("FoodService.DeleteHouseholdFood");

        if (error != null) return GlobalErrorHandler.handleError(error, logger);

        return Ok();
    }
}