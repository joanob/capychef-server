using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Capychef.Households.Domain.Entities;

public class StorageConditions
{
    private const string AmbientConst = "A";
    private const string RefrigeratedConst = "R";
    private const string FrozenConst = "F";

    private readonly string _value;

    private StorageConditions(string value)
    {
        _value = value;
    }

    public static StorageConditions AmbientTemperature => new(AmbientConst);
    public static StorageConditions Refrigerated => new(RefrigeratedConst);
    public static StorageConditions Frozen => new(FrozenConst);

    public static StorageConditions From(string value)
    {
        return value switch
        {
            AmbientConst => AmbientTemperature,
            RefrigeratedConst => Refrigerated,
            FrozenConst => Frozen,
            _ => new StorageConditions("")
        };
    }

    public override string ToString()
    {
        return _value;
    }
}

public class StorageConditionsConverter() : ValueConverter<StorageConditions, string>(v => v.ToString(),
    v => StorageConditions.From(v));