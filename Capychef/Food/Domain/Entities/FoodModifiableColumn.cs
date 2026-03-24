using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Capychef.Food.Domain.Entities;

public class FoodModifiableColumn
{
    private const string _nameColumn = "name";
    private const string _categoryIdColumn = "category_id";
    private const string _minQuantityColumn = "min_quantity";
    private const string _minQuantityUoMColumn = "min_quantity_uom";
    private const string _daysUntilExpirationColumn = "days_until_expiration";
    private const string _daysUntilBestBeforeColumn = "days_until_best_before";
    private const string _uoMColumn = "uom";

    private readonly string _value;

    private FoodModifiableColumn()
    {
    }

    private FoodModifiableColumn(string value)
    {
        _value = value;
    }

    public static FoodModifiableColumn Name => new(_nameColumn);
    public static FoodModifiableColumn CategoryId => new(_categoryIdColumn);
    public static FoodModifiableColumn MinQuantity => new(_minQuantityColumn);
    public static FoodModifiableColumn MinQuantityUoM => new(_minQuantityUoMColumn);
    public static FoodModifiableColumn DaysUntilExpiration => new(_daysUntilExpirationColumn);
    public static FoodModifiableColumn DaysUntilBestBefore => new(_daysUntilBestBeforeColumn);
    public static FoodModifiableColumn UoM => new(_uoMColumn);

    public static FoodModifiableColumn from(string value)
    {
        return value switch
        {
            _nameColumn => Name,
            _categoryIdColumn => CategoryId,
            _minQuantityColumn => MinQuantity,
            _minQuantityUoMColumn => MinQuantityUoM,
            _daysUntilExpirationColumn => DaysUntilExpiration,
            _daysUntilBestBeforeColumn => DaysUntilBestBefore,
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