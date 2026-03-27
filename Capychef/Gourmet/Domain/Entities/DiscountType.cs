using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Capychef.Gourmet.Domain.Entities;

public class DiscountType
{
    private const string PercentageConst = "PERCENTAJE";
    private const string FreeMonthConst = "FREE_MONTH";
    private const string FixedAmountConst = "FIXED_AMOUNT";

    private readonly string _value;

    private DiscountType(string value)
    {
        _value = value;
    }

    public static DiscountType Percentage => new(PercentageConst);
    public static DiscountType FreeMonth => new(FreeMonthConst);
    public static DiscountType FixedAmount => new(FixedAmountConst);

    public static DiscountType From(string value)
    {
        return value switch
        {
            PercentageConst => Percentage,
            FreeMonthConst => FreeMonth,
            FixedAmountConst => FixedAmount,
            _ => new DiscountType("")
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
            v => DiscountType.From(v)
        )
    {
    }
}