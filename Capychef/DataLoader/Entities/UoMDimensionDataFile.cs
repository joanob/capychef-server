using System.Text.Json.Serialization;

namespace Capychef.Data;

public class UoMDimensionDataFile
{
    [JsonPropertyName("code")] public string Code { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
}