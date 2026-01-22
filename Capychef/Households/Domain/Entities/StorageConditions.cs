using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Capychef.Households.Domain.Entities;

public class StorageConditions
{
    private const string _ambient = "A";
    private const string _refrigerated = "R";
    private const string _frozen = "F";

    private readonly string _value;

    private StorageConditions()
    {
    }

    private StorageConditions(string value)
    {
        _value = value;
    }

    public static StorageConditions AmbientTemperature => new(_ambient);
    public static StorageConditions Refrigerated => new(_refrigerated);
    public static StorageConditions Frozen => new(_frozen);

    public static StorageConditions from(string value)
    {
        return value switch
        {
            _ambient => AmbientTemperature,
            _refrigerated => Refrigerated,
            _frozen => Frozen,
            _ => null
        };
    }

    public override string ToString()
    {
        return _value;
    }
}

public class StorageConditionsConverter
    : ValueConverter<StorageConditions, string>
{
    public StorageConditionsConverter()
        : base(
            v => v.ToString(),
            v => StorageConditions.from(v)
        )
    {
    }
}