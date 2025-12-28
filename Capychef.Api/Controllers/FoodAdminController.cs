using Capychef.Food.Domain.Cmd;
using Capychef.Food.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("food-admin")]
public class FoodAdminController(IServiceScopeFactory serviceScopeFactory) : ControllerBase
{
    [HttpPost("categories")]
    public async Task<ActionResult> LoadFoodCategories(FoodCategoryFileCmd fileCmd)
    {
        Task.Run(async () =>
        {
            using var scope = serviceScopeFactory.CreateScope();

            var foodCategoryService = scope.ServiceProvider.GetRequiredService<IFoodCategoryService>();

            try
            {
                await foodCategoryService.LoadFoodCategories(fileCmd);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        });

        return Ok();
    }
}