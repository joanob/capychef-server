using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Capychef.Households.Domain.Entities;

public class StorageSpaceModifiableColumn
{
    private const string NameColumn = "name";
    private const string StorageConditionColumn = "storage_condition";

    private readonly string _value;

    private StorageSpaceModifiableColumn(string value)
    {
        _value = value;
    }

    public static StorageSpaceModifiableColumn Name => new(NameColumn);
    public static StorageSpaceModifiableColumn StorageCondition => new(StorageConditionColumn);

    public static StorageSpaceModifiableColumn From(string value)
    {
        return value switch
        {
            NameColumn => Name,
            StorageConditionColumn => StorageCondition,
            _ => new StorageSpaceModifiableColumn("")
        };
    }

    public override string ToString()
    {
        return _value;
    }
}

public class StorageSpaceModifiableColumnConverter
    : ValueConverter<StorageSpaceModifiableColumn, string>
{
    public StorageSpaceModifiableColumnConverter()
        : base(
            v => v.ToString(),
            v => StorageSpaceModifiableColumn.From(v)
        )
    {
    }
}