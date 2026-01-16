using System.ComponentModel.DataAnnotations.Schema;

namespace Capychef.Food.Domain.Entities;

[Table("food_uom")]
public class FoodUoM
{
    public FoodUoM()
    {
    }

    public FoodUoM(int foodId, string UoM, string? baseUoM, int? numerator, int? denominator)
    {
        FoodId = foodId;
        this.UoM = UoM;
        BaseUoM = baseUoM;
        Numerator = numerator;
        Denominator = denominator;
    }

    public FoodUoM(Food food, string UoM, string? baseUoM, int? numerator, int? denominator)
    {
        Food = food;
        this.UoM = UoM;
        BaseUoM = baseUoM;
        Numerator = numerator;
        Denominator = denominator;
    }

    [Column("id")] public int Id { get; private set; }

    [Column("food_id")] public int FoodId { get; set; }

    [Column("uom")] public string UoM { get; private set; }

    [Column("base_uom")] public string? BaseUoM { get; private set; }

    [Column("numerator")] public int? Numerator { get; private set; }

    [Column("denominator")] public int? Denominator { get; private set; }

    [ForeignKey(nameof(FoodId))] public Food Food { get; private set; }
}