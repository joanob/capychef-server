using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Capychef.Recipes.Domain.Entities;

public class RecipeType
{
    private const string GlobalConst = "G";
    private const string HouseholdConst = "H";
    private const string PublicConst = "P";
    private const string DraftConst = "D";

    private readonly string _value;

    private RecipeType(string value)
    {
        _value = value;
    }

    public static RecipeType Global => new(GlobalConst);
    public static RecipeType Household => new(HouseholdConst);
    public static RecipeType Public => new(PublicConst);
    public static RecipeType Draft => new(DraftConst);

    public static RecipeType From(string value)
    {
        return value switch
        {
            GlobalConst => Global,
            HouseholdConst => Household,
            PublicConst => Public,
            DraftConst => Draft,
            _ => new RecipeType("")
        };
    }

    public override string ToString()
    {
        return _value;
    }
}

public class RecipeTypeConverter() : ValueConverter<RecipeType, string>(v => v.ToString(),
    v => RecipeType.From(v));