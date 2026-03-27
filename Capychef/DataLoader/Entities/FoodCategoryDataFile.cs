using System.Text.Json.Serialization;

namespace Capychef.DataLoader.Entities;

public class FoodCategoryDataFile
{
    [JsonPropertyName("id")] public int Id { get; init; }

    [JsonPropertyName("name")] public required string Name { get; init; }

    [JsonPropertyName("children")] public List<FoodCategoryDataFile>? Children { get; init; }
}