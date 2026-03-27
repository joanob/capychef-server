using System.Text.Json.Serialization;

namespace Capychef.DataLoader.Entities;

public class CapychefDataFile
{
    [JsonPropertyName("initialStorageSpaces")]
    public List<InitialStorageSpaceDataFile>? InitialStorageSpaces { get; init; }

    [JsonPropertyName("uomDimensions")] public List<UoMDimensionDataFile>? UomDimensions { get; init; }

    [JsonPropertyName("uom")] public List<UoMDataFile>? Uom { get; init; }

    [JsonPropertyName("foodCategories")] public List<FoodCategoryDataFile>? FoodCategories { get; init; }

    [JsonPropertyName("food")] public List<FoodDataFile>? Food { get; init; }
}