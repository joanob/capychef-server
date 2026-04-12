using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Capychef.Storage.Domain.Entities;

public class BatchModificationType
{
    private const string InitialConst = "I";
    private const string UpdateQuantityConst = "Q";
    private const string UpdateBestBeforeDateConst = "B";
    private const string UpdateExpirationDateConst = "e";
    private const string FullMoveConst = "M";
    private const string PartialMoveConst = "P";
    private const string FullConsumeConst = "C";
    private const string PartialConsumeConst = "K";
    private const string FullDiscardConst = "D";
    private const string PartialDiscardConst = "T";

    private readonly string _value;

    private BatchModificationType(string value)
    {
        _value = value;
    }

    public static BatchModificationType Initial => new(InitialConst);
    public static BatchModificationType UpdateQuantity => new(UpdateQuantityConst);
    public static BatchModificationType UpdateBestBeforeDate => new(UpdateBestBeforeDateConst);
    public static BatchModificationType UpdateExpirationDate => new(UpdateExpirationDateConst);
    public static BatchModificationType FullMove => new(FullMoveConst);
    public static BatchModificationType PartialMove => new(PartialMoveConst);
    public static BatchModificationType FullConsume => new(FullConsumeConst);
    public static BatchModificationType PartialConsume => new(PartialConsumeConst);
    public static BatchModificationType FullDiscard => new(FullDiscardConst);
    public static BatchModificationType PartialDiscard => new(PartialDiscardConst);

    public static BatchModificationType From(string value)
    {
        return value switch
        {
            InitialConst => Initial,
            UpdateQuantityConst => UpdateQuantity,
            UpdateBestBeforeDateConst => UpdateBestBeforeDate,
            UpdateExpirationDateConst => UpdateExpirationDate,
            FullMoveConst => FullMove,
            PartialMoveConst => PartialMove,
            FullConsumeConst => FullConsume,
            PartialConsumeConst => PartialConsume,
            FullDiscardConst => FullDiscard,
            PartialDiscardConst => PartialDiscard,
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