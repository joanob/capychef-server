using System.Text.Json.Serialization;

namespace Capychef.Data;

public class FoodCategoryDataFile
{
    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("children")] public List<FoodCategoryDataFile>? Children { get; set; }
}