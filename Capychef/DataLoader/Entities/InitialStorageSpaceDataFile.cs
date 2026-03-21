using System.Text.Json.Serialization;

namespace Capychef.Data;

public class InitialStorageSpaceDataFile
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("storageCondition")] public string StorageCondition { get; set; }
}