using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Capychef.Food.Domain.Entities;

public class FoodUoMModifiableColumn
{
    private const string IsBaseColumn = "is_base";
    private const string BaseUoMColumn = "base_uom";
    private const string NumeratorColumn = "numerator";
    private const string DenominatorColumn = "denominator";
    private const string IsApproxConversionColumn = "is_approx_conversion";

    private readonly string _value;

    private FoodUoMModifiableColumn(string value)
    {
        _value = value;
    }

    public static FoodUoMModifiableColumn IsBase => new(IsBaseColumn);
    public static FoodUoMModifiableColumn BaseUoM => new(BaseUoMColumn);
    public static FoodUoMModifiableColumn Numerator => new(NumeratorColumn);
    public static FoodUoMModifiableColumn Denominator => new(DenominatorColumn);
    public static FoodUoMModifiableColumn IsApproxConversion => new(IsApproxConversionColumn);

    public static FoodUoMModifiableColumn From(string value)
    {
        return value switch
        {
            IsBaseColumn => IsBase,
            BaseUoMColumn => BaseUoM,
            NumeratorColumn => Numerator,
            DenominatorColumn => Denominator,
            IsApproxConversionColumn => IsApproxConversion,
            _ => new FoodUoMModifiableColumn("")
        };
    }

    public override string ToString()
    {
        return _value;
    }
}

public class FoodUoMModifiableColumnConverter
    : ValueConverter<FoodUoMModifiableColumn, string>
{
    public FoodUoMModifiableColumnConverter()
        : base(
            v => v.ToString(),
            v => FoodUoMModifiableColumn.From(v)
        )
    {
    }
}