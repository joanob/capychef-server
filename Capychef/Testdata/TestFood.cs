using Capychef.Common.Utils;
using Capychef.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Testdata;

public class TestFood(CapychefDbContext dbContext)
{
    private readonly float DELETED_FOOD_PERCENTAJE = 0.2f;
    private readonly int MAX_FOOD_PER_HOUSEHOLD = 20;
    private readonly int MIN_FOOD_PER_HOUSEHOLD = 0;

    public async Task Generate()
    {
        Console.WriteLine("Generating food");

        var households = await dbContext.Households.ToListAsync();

        var foodCategories = await dbContext.FoodCategories.ToListAsync();

        var unitsOfMeasure = await dbContext.UoM.ToListAsync();

        foreach (var household in households)
        {
            var householdFoodNumber =
                RandomGenerator.GenerateRandomNumber(MAX_FOOD_PER_HOUSEHOLD - MIN_FOOD_PER_HOUSEHOLD) +
                MIN_FOOD_PER_HOUSEHOLD;

            var householdMembers =
                await dbContext.HouseholdMembers.Where(x => x.HouseholdId == household.Id).ToListAsync();

            for (var i = 0; i < householdFoodNumber; i++)
            {
                var foodCategory = foodCategories.ElementAt(RandomGenerator.GenerateRandomNumber(foodCategories.Count));

                var uom = unitsOfMeasure.ElementAt(RandomGenerator.GenerateRandomNumber(unitsOfMeasure.Count));

                var createdBy =
                    householdMembers.ElementAt(RandomGenerator.GenerateRandomNumber(householdMembers.Count));

                var food = new Food.Domain.Entities.Food(household.Id, RandomGenerator.GenerateRandomAlphabetString(10),
                    foodCategory.Id, uom.Code, createdBy.UserId);

                if (RandomGenerator.GenerateRandomBoolPercentage(DELETED_FOOD_PERCENTAJE))
                    food.Delete();

                await dbContext.AddAsync(food);
            }
        }

        await dbContext.SaveChangesAsync();

        Console.WriteLine("Food generated");
    }
}