using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace YourOwnBoss.Common.Entities;

public class FoodModifiableColumn
{
    private const string _nameColumn = "name";
    private const string _categoryIdColumn = "category_id";

    private readonly string _value;

    private FoodModifiableColumn(string value)
    {
        _value = value;
    }

    public static FoodModifiableColumn Name => new(_nameColumn);
    public static FoodModifiableColumn CategoryId => new(_categoryIdColumn);

    public static FoodModifiableColumn from(string value)
    {
        return value switch
        {
            _nameColumn => Name,
            _categoryIdColumn => CategoryId,
            _ => null
        };
    }

    public override string ToString()
    {
        return _value;
    }
}

public class FoodModifiableColumnConverter
    : ValueConverter<FoodModifiableColumn, string>
{
    public FoodModifiableColumnConverter()
        : base(
            v => v.ToString(),
            v => FoodModifiableColumn.from(v)
        )
    {
    }
}