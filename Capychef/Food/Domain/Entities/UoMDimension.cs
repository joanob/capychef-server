using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capychef.Food.Domain.Entities;

[Table("uom_dimensions")]
public class UoMDimension
{
    public UoMDimension()
    {
    }

    public UoMDimension(string code, string name)
    {
        Code = code;
        Name = name;
    }

    [Key] [Column("code")] public string Code { get; private set; }

    [Column("name")] public string Name { get; private set; }
}