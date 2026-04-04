using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capychef.Food.Domain.Entities;

[Table("uom_dimensions")]
public class UoMDimension
{
    protected UoMDimension()
    {
        Code = "";
        Name = "";
    }

    public UoMDimension(string code, string name)
    {
        Code = code;
        Name = name;
    }

    [Key] [Column("code")] [MaxLength(4)] public string Code { get; init; }

    [Column("name")] [MaxLength(50)] public string Name { get; private set; }

    public void Set(UoMDimension uoMDimension)
    {
        Name = uoMDimension.Name;
    }
}