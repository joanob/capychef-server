using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Capychef.Households.Domain.Entities;

public class BatchModificationType
{
    private const string InitialConst = "C";

    private readonly string _value;

    private BatchModificationType(string value)
    {
        _value = value;
    }

    public static BatchModificationType Initial => new(InitialConst);

    public static BatchModificationType From(string value)
    {
        return value switch
        {
            InitialConst => Initial,
            _ => new BatchModificationType("")
        };
    }

    public override string ToString()
    {
        return _value;
    }
}

public class BatchModificationTypeConverter() : ValueConverter<BatchModificationType, string>(v => v.ToString(),
    v => BatchModificationType.From(v));