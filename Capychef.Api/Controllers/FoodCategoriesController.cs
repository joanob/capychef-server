using Capychef.Food.Domain.Cmd;
using Capychef.Food.Domain.DTO;
using Capychef.Food.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Capychef.Api.Controllers;

[ApiController]
[Route("food-categories")]
public class FoodCategoriesController(
    IServiceScopeFactory serviceScopeFactory,
    IFoodCategoryService foodCategoryService) : ControllerBase
{
    [HttpPost]
    public ActionResult LoadFoodCategories(FoodCategoryFileCmd fileCmd)
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

    [HttpGet]
    public async Task<ActionResult<List<FoodCategoryDto>>> GetAllFoodCategories([FromQuery] string? displayMode)
    {
        if (displayMode == "tree") return Ok(await foodCategoryService.GetAllCategoriesAsTree());

        return Ok(await foodCategoryService.GetAllCategories());
    }
}