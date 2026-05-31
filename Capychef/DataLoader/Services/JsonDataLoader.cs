using System.Text.Json;
using Capychef.DataLoader.Entities;
using Capychef.Food.Domain.Entities;
using Capychef.Households.Domain.Entities;
using Capychef.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Capychef.DataLoader.Services;

public class JsonDataLoader(CapychefDbContext dbContext)
    : IDataLoader
{
    public async Task LoadFromStream(Stream stream)
    {
        if (stream.CanSeek)
            stream.Position = 0;

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
        };

        var data = await JsonSerializer.DeserializeAsync<CapychefDataFile>(stream, options);

        if (data == null) return;

        await LoadInitialStorageSpaces(data.InitialStorageSpaces);

        var uoMDimensions = await LoadUoMDimensions(data.UomDimensions);

        var uoMs = await LoadUoMs(data.Uom, uoMDimensions);

        var foodCategories = await LoadFoodCategories(data.FoodCategories);

        await LoadFood(data.Food, foodCategories, uoMs);

        await dbContext.SaveChangesAsync();

        Console.WriteLine("Data loaded successfully from JSON file");
    }

    private async Task LoadInitialStorageSpaces(List<InitialStorageSpaceDataFile>? initialStorageSpacesData)
    {
        if (initialStorageSpacesData == null || initialStorageSpacesData.Count == 0) return;

        var storedInitialStorageSpaces = await dbContext.InitialStorageSpaces.ToListAsync();

        var initialStorageSpaces = initialStorageSpacesData
            .Select(x => new InitialStorageSpace(x.Id, x.Name, StorageConditions.From(x.StorageCondition))).ToList();

        foreach (var initialStorageSpace in initialStorageSpaces)
        {
            var storedInitialStorageSpace =
                storedInitialStorageSpaces.FirstOrDefault(x => x.Id == initialStorageSpace.Id);
            if (storedInitialStorageSpace == null)
            {
                await dbContext.InitialStorageSpaces.AddAsync(initialStorageSpace);
            }
            else
            {
                storedInitialStorageSpace.Name = initialStorageSpace.Name;
                storedInitialStorageSpace.StorageCondition = initialStorageSpace.StorageCondition;
            }
        }
    }

    private async Task<List<UoMDimension>> LoadUoMDimensions(List<UoMDimensionDataFile>? uoMDimensionsData)
    {
        if (uoMDimensionsData == null || uoMDimensionsData.Count == 0)
            return new List<UoMDimension>();

        var storedUoMDimensions = await dbContext.UoMDimensions.ToListAsync();

        var uomDimensions = uoMDimensionsData
            .Select(x => new UoMDimension(x.Code, x.Name)).ToList();

        foreach (var uomDimension in uomDimensions)
        {
            var storedUoMDimension = storedUoMDimensions.FirstOrDefault(x => x.Code == uomDimension.Code);
            if (storedUoMDimension == null)
            {
                await dbContext.UoMDimensions.AddAsync(uomDimension);

                storedUoMDimensions.Add(uomDimension);
            }
            else
            {
                storedUoMDimension.Set(uomDimension);
            }
        }

        return storedUoMDimensions;
    }

    private async Task<List<UoM>> LoadUoMs(List<UoMDataFile>? uoMsData, List<UoMDimension> uoMDimensions)
    {
        if (uoMsData == null || uoMsData.Count == 0)
            return new List<UoM>();

        var storedUoMs = await dbContext.UoM.ToListAsync();

        var uoMs = uoMsData
            .Select(x => new UoM(x.Code, x.Name, x.DimensionCode, x.BaseUom, x.Numerator, x.Denominator)).ToList();

        foreach (var uoM in uoMs)
        {
            if (uoMDimensions.All(d => d.Code != uoM.DimensionCode))
                throw new Exception($"UoM {uoM.Code} has a dimension code {uoM.DimensionCode} that does not exist");

            var storedUoM = storedUoMs.FirstOrDefault(x => x.Code == uoM.Code);
            if (storedUoM == null)
            {
                await dbContext.UoM.AddAsync(uoM);

                storedUoMs.Add(uoM);
            }
            else
            {
                storedUoM.Set(uoM);
            }
        }

        return storedUoMs;
    }

    /**
     * Food categories is a tree with max depth 3.
     *
     * Categories that don't have any children are leaf categories
     */
    private async Task<List<FoodCategory>> LoadFoodCategories(List<FoodCategoryDataFile>? foodCategoriesData)
    {
        if (foodCategoriesData == null || foodCategoriesData.Count == 0)
            return new List<FoodCategory>();

        var storedFoodCategories = await dbContext.FoodCategories.ToListAsync();

        foreach (var foodCategoryLevel1Data in foodCategoriesData)
        {
            var isLeaf1 = !(foodCategoryLevel1Data.Children?.Any() ?? false);
            var foodCategoryLevel1 = new FoodCategory(foodCategoryLevel1Data.Id, foodCategoryLevel1Data.Name,
                isLeaf1, null);

            var storedCategory = storedFoodCategories.FirstOrDefault(x => x.Id == foodCategoryLevel1Data.Id);
            if (storedCategory == null)
            {
                await dbContext.FoodCategories.AddAsync(foodCategoryLevel1);

                storedFoodCategories.Add(foodCategoryLevel1);
            }
            else
            {
                storedCategory.Set(foodCategoryLevel1);
            }

            foreach (var foodCategoryLevel2Data in foodCategoryLevel1Data.Children ??
                                                   Enumerable.Empty<FoodCategoryDataFile>())
            {
                var isLeaf2 = !(foodCategoryLevel2Data.Children?.Any() ?? false);
                var foodCategoryLevel2 = new FoodCategory(foodCategoryLevel2Data.Id, foodCategoryLevel2Data.Name,
                    isLeaf2, foodCategoryLevel1.Id);

                storedCategory = storedFoodCategories.FirstOrDefault(x => x.Id == foodCategoryLevel2Data.Id);
                if (storedCategory == null)
                {
                    await dbContext.FoodCategories.AddAsync(foodCategoryLevel2);

                    storedFoodCategories.Add(foodCategoryLevel2);
                }
                else
                {
                    storedCategory.Set(foodCategoryLevel2);
                }

                foreach (var foodCategoryLevel3Data in foodCategoryLevel2Data.Children ??
                                                       Enumerable.Empty<FoodCategoryDataFile>())
                {
                    var isLeaf3 = !(foodCategoryLevel3Data.Children?.Any() ?? false);
                    var foodCategoryLevel3 = new FoodCategory(foodCategoryLevel3Data.Id, foodCategoryLevel3Data.Name,
                        isLeaf3, foodCategoryLevel2.Id);

                    storedCategory = storedFoodCategories.FirstOrDefault(x => x.Id == foodCategoryLevel3Data.Id);
                    if (storedCategory == null)
                    {
                        await dbContext.FoodCategories.AddAsync(foodCategoryLevel3);

                        storedFoodCategories.Add(foodCategoryLevel3);
                    }
                    else
                    {
                        storedCategory.Set(foodCategoryLevel3);
                    }

                    if (foodCategoryLevel3Data.Children != null && foodCategoryLevel3Data.Children.Any())
                        throw new Exception(
                            $"Food Category {foodCategoryLevel3Data.Id} is level 3 and cannot have children");
                }
            }
        }

        return storedFoodCategories;
    }


    private async Task LoadFood(List<FoodDataFile>? foodDataList, List<FoodCategory> foodCategories, List<UoM> uoMs)
    {
        if (foodDataList == null || foodDataList.Count == 0)
            return;

        var storedFoodList = await dbContext.Food.ToListAsync();

        foreach (var foodData in foodDataList)
        {
            // Check food category
            if (!foodCategories.Any(fc => fc.Id == foodData.Category && fc.IsLeaf))
                throw new Exception(
                    $"Food {foodData.GlobalId} has a category {foodData.Category} that does not exist or is not a leaf category");

            // Check one food uom is base uom
            if (foodData.UnitsOfMeasure.Count(u => u.IsBaseUoM ?? false) != 1)
                throw new Exception($"Food {foodData.GlobalId} must have exactly one base UoM");

            if (foodData.DaysUntilBestBefore is < 0 ||
                foodData.DaysUntilExpiration is < 0)
                throw new Exception(
                    $"Food {foodData.GlobalId} days until best before or expiration cannot be negative");

            var food = new Food.Domain.Entities.Food(foodData.GlobalId, foodData.Name, foodData.Category,
                foodData.DaysUntilExpiration, foodData.DaysUntilBestBefore);

            var storedFood = storedFoodList.FirstOrDefault(x => x.GlobalId == foodData.GlobalId);
            if (storedFood == null)
            {
                await dbContext.Food.AddAsync(food);

                storedFoodList.Add(food);

                foreach (var foodDataUoM in foodData.UnitsOfMeasure)
                {
                    if (uoMs.All(u => u.Code != foodDataUoM.UoM))
                        throw new Exception(
                            $"Food {foodData.GlobalId} has a UoM {foodDataUoM.UoM} that does not exist");

                    await dbContext.FoodUoM.AddAsync(new FoodUoM(food, null, foodDataUoM.UoM,
                        foodDataUoM.IsBaseUoM ?? false, foodDataUoM.Numerator, foodDataUoM.Denominator,
                        foodDataUoM.IsApproxConversion));
                }
            }
            else
            {
                storedFood.Set(food);

                // Iterate first units of measure that are not base uom to set current base uom as false before setting new base uom
                foreach (var foodDataUoM in foodData.UnitsOfMeasure.Where(x =>
                             !x.IsBaseUoM.HasValue || !x.IsBaseUoM.Value).ToList())
                {
                    if (uoMs.All(u => u.Code != foodDataUoM.UoM))
                        throw new Exception(
                            $"Food {foodData.GlobalId} has a UoM {foodDataUoM.UoM} that does not exist");

                    var storedFoodUoM =
                        await dbContext.FoodUoM.FirstOrDefaultAsync(x =>
                            x.FoodId == storedFood.Id && x.UoM == foodDataUoM.UoM);

                    if (storedFoodUoM == null)
                    {
                        await dbContext.FoodUoM.AddAsync(new FoodUoM(storedFood, null, foodDataUoM.UoM,
                            foodDataUoM.IsBaseUoM ?? false,
                            foodDataUoM.Numerator, foodDataUoM.Denominator, foodDataUoM.IsApproxConversion));
                    }
                    else
                    {
                        var isBaseUoM = foodDataUoM.IsBaseUoM.HasValue && foodDataUoM.IsBaseUoM.Value;
                        storedFoodUoM.Set(isBaseUoM, foodDataUoM.Numerator,
                            foodDataUoM.Denominator, foodDataUoM.IsApproxConversion);
                    }
                }

                foreach (var foodDataUoM in
                         foodData.UnitsOfMeasure.Where(x => x.IsBaseUoM.HasValue && x.IsBaseUoM.Value).ToList())
                {
                    if (uoMs.All(u => u.Code != foodDataUoM.UoM))
                        throw new Exception(
                            $"Food {foodData.GlobalId} has a UoM {foodDataUoM.UoM} that does not exist");

                    var storedFoodUoM =
                        await dbContext.FoodUoM.FirstOrDefaultAsync(x =>
                            x.FoodId == storedFood.Id && x.UoM == foodDataUoM.UoM);

                    if (storedFoodUoM == null)
                    {
                        await dbContext.FoodUoM.AddAsync(new FoodUoM(storedFood, null, foodDataUoM.UoM,
                            foodDataUoM.IsBaseUoM ?? false,
                            foodDataUoM.Numerator, foodDataUoM.Denominator, foodDataUoM.IsApproxConversion));
                    }
                    else
                    {
                        var isBaseUoM = foodDataUoM.IsBaseUoM.HasValue && foodDataUoM.IsBaseUoM.Value;
                        storedFoodUoM.Set(isBaseUoM, foodDataUoM.Numerator,
                            foodDataUoM.Denominator, foodDataUoM.IsApproxConversion);
                    }
                }
            }
        }
    }
}