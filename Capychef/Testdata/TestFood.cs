using Capychef.Common.Utils;
using Capychef.Food.Domain.Entities;
using Capychef.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Testdata;

public class TestFood(CapychefDbContext dbContext)
{
    private readonly float _deletedFoodPercentaje = 0.2f;
    private readonly int _maxFoodPerHousehold = 20;
    private readonly int _minFoodPerHousehold = 0;

    public async Task Generate()
    {
        Console.WriteLine("Generating food");

        var households = await dbContext.Households.ToListAsync();

        var foodLeafCategories = await dbContext.FoodCategories.Where(x => x.IsLeaf).ToListAsync();

        var unitsOfMeasure = await dbContext.UoM.ToListAsync();

        foreach (var household in households)
        {
            var householdFoodNumber =
                RandomGenerator.GenerateRandomNumber(_maxFoodPerHousehold - _minFoodPerHousehold) +
                _minFoodPerHousehold;

            var householdMembers =
                await dbContext.HouseholdMembers.Where(x => x.HouseholdId == household.Id).ToListAsync();

            for (var i = 0; i < householdFoodNumber; i++)
            {
                var foodCategory =
                    foodLeafCategories.ElementAt(RandomGenerator.GenerateRandomNumber(foodLeafCategories.Count));

                var uom = unitsOfMeasure.ElementAt(RandomGenerator.GenerateRandomNumber(unitsOfMeasure.Count));

                var createdBy =
                    householdMembers.ElementAt(RandomGenerator.GenerateRandomNumber(householdMembers.Count));

                var food = new Food.Domain.Entities.Food(household.Id, RandomGenerator.GenerateRandomAlphabetString(10),
                    foodCategory.Id, createdBy.UserId);

                if (RandomGenerator.GenerateRandomBoolPercentage(_deletedFoodPercentaje))
                    food.Delete();

                var foodUoM = new FoodUoM(food, household.Id, uom.Code, false, null, null, null);

                await dbContext.AddAsync(food);

                await dbContext.AddAsync(foodUoM);
            }
        }

        await dbContext.SaveChangesAsync();

        Console.WriteLine("Food generated");
    }
}