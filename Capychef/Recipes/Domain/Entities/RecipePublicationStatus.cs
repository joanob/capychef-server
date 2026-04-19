using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Capychef.Recipes.Domain.Entities;

public class RecipePublicationStatus
{
    private const string PendingConst = "PENDING";
    private const string ApprovedConst = "APPROVED";
    private const string RejectedConst = "REJECTED";
    private const string ActiveConst = "ACTIVE";

    private readonly string _value;

    private RecipePublicationStatus(string value)
    {
        _value = value;
    }

    public static RecipePublicationStatus Pending => new(PendingConst);
    public static RecipePublicationStatus Approved => new(ApprovedConst);
    public static RecipePublicationStatus Rejected => new(RejectedConst);
    public static RecipePublicationStatus Active => new(ActiveConst);

    public static RecipePublicationStatus From(string value)
    {
        return value switch
        {
            PendingConst => Pending,
            ApprovedConst => Approved,
            RejectedConst => Rejected,
            ActiveConst => Active,
            _ => new RecipePublicationStatus("")
        };
    }

    public override string ToString()
    {
        return _value;
    }
}

public class RecipePublicationStatusConverter() : ValueConverter<RecipePublicationStatus, string>(
    v => v.ToString(),
    v => RecipePublicationStatus.From(v));