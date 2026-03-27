using System.Text.Json.Serialization;

namespace Capychef.DataLoader.Entities;

public class InitialStorageSpaceDataFile
{
    [JsonPropertyName("id")] public int Id { get; init; }
    [JsonPropertyName("name")] public required string Name { get; init; }
    [JsonPropertyName("storageCondition")] public required string StorageCondition { get; init; }
}