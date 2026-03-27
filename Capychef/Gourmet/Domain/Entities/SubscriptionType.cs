using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Capychef.Gourmet.Domain.Entities;

public class SubscriptionType
{
    private const string MonthlyConst = "M";
    private const string AnnuallyConst = "A";

    private readonly string _value;

    private SubscriptionType(string value)
    {
        _value = value;
    }

    public static SubscriptionType Monthly => new(MonthlyConst);
    public static SubscriptionType Annually => new(AnnuallyConst);

    public static SubscriptionType From(string value)
    {
        return value switch
        {
            MonthlyConst => Monthly,
            AnnuallyConst => Annually,
            _ => new SubscriptionType("")
        };
    }

    public override string ToString()
    {
        return _value;
    }
}

public class SubscriptionTypeConverter
    : ValueConverter<SubscriptionType, string>
{
    public SubscriptionTypeConverter()
        : base(
            v => v.ToString(),
            v => SubscriptionType.From(v)
        )
    {
    }
}