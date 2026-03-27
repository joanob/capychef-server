using System.Text.Json.Serialization;

namespace Capychef.DataLoader.Entities;

public class UoMDimensionDataFile
{
    [JsonPropertyName("code")] public required string Code { get; init; }
    [JsonPropertyName("name")] public required string Name { get; init; }
}