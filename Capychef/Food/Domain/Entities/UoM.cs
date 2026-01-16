using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capychef.Food.Domain.Entities;

[Table("uom")]
public class UoM
{
    public UoM()
    {
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

    [Key] [Column("code")] public string Code { get; private set; }

    [Column("name")] public string Name { get; private set; }

    [Column("dimension")]
    [ForeignKey(nameof(Dimension))]
    public string DimensionCode { get; set; }

    [Column("base_uom")] public string? BaseUoM { get; private set; }

    [Column("numerator")] public int? Numerator { get; private set; }

    [Column("denominator")] public int? Denominator { get; private set; }

    public UoMDimension Dimension { get; private set; }
}