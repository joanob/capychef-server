using Capychef.Food.Domain.Cmd;
using Capychef.Food.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("food")]
public class FoodController(
    IServiceScopeFactory serviceScopeFactory,
    IFoodService foodService) : ControllerBase
{
    [HttpPost]
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