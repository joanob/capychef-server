using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capychef.Food.Domain.Entities;

[Table("uom")]
public class UoM
{
    protected UoM()
    {
        Code = "";
        Name = "";
        DimensionCode = "";
    }

    public UoM(string code, string name, string dimensionCode, string? baseUoM, int? numerator, int? denominator)
    {
        Code = code;
        Name = name;
        DimensionCode = dimensionCode;
        BaseUoM = baseUoM;
        Numerator = numerator;
        Denominator = denominator;
    }

    [Key] [Column("code")] [MaxLength(4)] public string Code { get; init; }

    [Column("name")] [MaxLength(50)] public string Name { get; private set; }

    [Column("dimension")] [MaxLength(10)] public string DimensionCode { get; private set; }

    [Column("base_uom")] [MaxLength(4)] public string? BaseUoM { get; private set; }

    [Column("numerator")] public int? Numerator { get; private set; }

    [Column("denominator")] public int? Denominator { get; private set; }

    [ForeignKey(nameof(DimensionCode))] public UoMDimension? Dimension { get; init; }

    public void Set(UoM uoM)
    {
        Name = uoM.Name;
        DimensionCode = uoM.DimensionCode;
        BaseUoM = uoM.BaseUoM;
        Numerator = uoM.Numerator;
        Denominator = uoM.Denominator;
    }
}