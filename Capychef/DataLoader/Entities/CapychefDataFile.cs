using System.Text.Json.Serialization;

namespace Capychef.Data;

public class CapychefDataFile
{
    [JsonPropertyName("uomDimensions")] public List<UoMDimensionDataFile>? UomDimensions { get; set; }

    [JsonPropertyName("uom")] public List<UoMDataFile>? Uom { get; set; }

    [JsonPropertyName("foodCategories")] public List<FoodCategoryDataFile>? FoodCategories { get; set; }

    [JsonPropertyName("food")] public List<FoodDataFile>? Food { get; set; }
}