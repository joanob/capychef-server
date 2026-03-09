using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Capychef.Common.Errors;

public class DiscountType
{
    private const string _percentage = "PERCENTAJE";
    private const string _free_month = "FREE_MONTH";
    private const string _fixed_amount = "FIXED_AMOUNT";

    private readonly string _value;

    private DiscountType(string value)
    {
        _value = value;
    }

    public static DiscountType Percentage => new(_percentage);
    public static DiscountType FreeMonth => new(_free_month);
    public static DiscountType FixedAmount => new(_fixed_amount);

    public static DiscountType from(string value)
    {
        return value switch
        {
            _percentage => Percentage,
            _free_month => FreeMonth,
            _fixed_amount => FixedAmount,
            _ => null
        };
    }

    public override string ToString()
    {
        return _value;
    }
}

public class DiscountTypeConverter
    : ValueConverter<DiscountType, string>
{
    public DiscountTypeConverter()
        : base(
            v => v.ToString(),
            v => DiscountType.from(v)
        )
    {
    }
}