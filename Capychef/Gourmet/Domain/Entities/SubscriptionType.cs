using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Capychef.Common.Errors;

public class SubscriptionType
{
    private const string _monthly = "M";
    private const string _annually = "A";

    private readonly string _value;

    private SubscriptionType(string value)
    {
        _value = value;
    }

    public static SubscriptionType Monthly => new(_monthly);
    public static SubscriptionType Annually => new(_annually);

    public static SubscriptionType from(string value)
    {
        return value switch
        {
            _monthly => Monthly,
            _annually => Annually,
            _ => null
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
            v => SubscriptionType.from(v)
        )
    {
    }
}