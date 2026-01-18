using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace YourOwnBoss.Common.Entities;

public class StorageSpaceModifiableColumn
{
    private const string _nameColumn = "name";
    private const string _storageConditionColumn = "storage_condition";

    private readonly string _value;

    private StorageSpaceModifiableColumn()
    {
    }

    private StorageSpaceModifiableColumn(string value)
    {
        _value = value;
    }

    public static StorageSpaceModifiableColumn Name => new(_nameColumn);
    public static StorageSpaceModifiableColumn StorageCondition => new(_storageConditionColumn);

    public static StorageSpaceModifiableColumn from(string value)
    {
        return value switch
        {
            _nameColumn => Name,
            _storageConditionColumn => StorageCondition,
            _ => null
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
            v => StorageSpaceModifiableColumn.from(v)
        )
    {
    }
}