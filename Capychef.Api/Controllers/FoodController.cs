using Capychef.Api.Authorization;
using Capychef.Food.Domain.Cmd;
using Capychef.Food.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using YourOwnBoss.Common.Auth;
using YourOwnBoss.Common.Result;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("food")]
public class FoodController(
    IServiceScopeFactory serviceScopeFactory,
    IFoodService foodService) : ControllerBase
{
    [CheckMembership]
    [HttpPost]
    public async Task<ActionResult<FoodDTO>> CreateHouseholdFood(CreateHouseholdFoodCmd cmd)
    {
        var userDetails = AuthUserDetailsService.GetAuthUserDetailsFromContext(HttpContext);

        var food = await foodService.CreateHouseholdFood(userDetails, cmd);

        if (food.failed()) return GlobalErrorHandler.handleError(food.error());

        return Ok(food.get());
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

    [HttpGet]
    public async Task<ActionResult<List<FoodCategoryDTO>>> GetAllFoodCategories()
    {
        return Ok(await foodService.GetAllGlobalFood());
    }
}