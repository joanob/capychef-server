using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Capychef.Food.Domain.Entities;

public class FoodModifiableColumn
{
    private const string NameColumn = "name";
    private const string CategoryIdColumn = "category_id";
    private const string MinQuantityColumn = "min_quantity";
    private const string MinQuantityUoMColumn = "min_quantity_uom";
    private const string DaysUntilExpirationColumn = "days_until_expiration";
    private const string DaysUntilBestBeforeColumn = "days_until_best_before";
    private const string UoMColumn = "uom";

    private readonly string _value;

    private FoodModifiableColumn(string value)
    {
        _value = value;
    }

    public static FoodModifiableColumn Name => new(NameColumn);
    public static FoodModifiableColumn CategoryId => new(CategoryIdColumn);
    public static FoodModifiableColumn MinQuantity => new(MinQuantityColumn);
    public static FoodModifiableColumn MinQuantityUoM => new(MinQuantityUoMColumn);
    public static FoodModifiableColumn DaysUntilExpiration => new(DaysUntilExpirationColumn);
    public static FoodModifiableColumn DaysUntilBestBefore => new(DaysUntilBestBeforeColumn);
    public static FoodModifiableColumn UoM => new(UoMColumn);

    public static FoodModifiableColumn From(string value)
    {
        return value switch
        {
            NameColumn => Name,
            CategoryIdColumn => CategoryId,
            MinQuantityColumn => MinQuantity,
            MinQuantityUoMColumn => MinQuantityUoM,
            DaysUntilExpirationColumn => DaysUntilExpiration,
            DaysUntilBestBeforeColumn => DaysUntilBestBefore,
            UoMColumn => UoM,
            _ => new FoodModifiableColumn("")
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
            v => FoodModifiableColumn.From(v)
        )
    {
    }
}