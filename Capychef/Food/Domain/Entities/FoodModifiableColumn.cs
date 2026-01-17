using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace YourOwnBoss.Common.Entities;

public class FoodModifiableColumn
{
    private const string _nameColumn = "name";
    private const string _categoryIdColumn = "category_id";
    private const string _baseUoMColumn = "base_uom";
    private const string _uoMColumn = "uom";

    private readonly string _value;

    private FoodModifiableColumn(string value)
    {
        _value = value;
    }

    public static FoodModifiableColumn Name => new(_nameColumn);
    public static FoodModifiableColumn CategoryId => new(_categoryIdColumn);
    public static FoodModifiableColumn BaseUoM => new(_baseUoMColumn);
    public static FoodModifiableColumn UoM => new(_uoMColumn);

    public static FoodModifiableColumn from(string value)
    {
        return value switch
        {
            _nameColumn => Name,
            _categoryIdColumn => CategoryId,
            _baseUoMColumn => BaseUoM,
            _uoMColumn => UoM,
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