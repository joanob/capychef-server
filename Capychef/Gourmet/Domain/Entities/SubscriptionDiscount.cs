using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Gourmet.Domain.Entities;

[Table("subscription_discounts")]
public class SubscriptionDiscount
{
    protected SubscriptionDiscount()
    {
        Name = null!;
        DiscountType = null!;
    }

    public SubscriptionDiscount(string discountCode, DiscountType discountType, double percentageOrAmount,
        DateTime validFrom, DateTime validUntil)
    {
        Name = discountCode;
        DiscountType = discountType;
        PercentageOrAmount = percentageOrAmount;
        ValidFrom = validFrom;
        ValidUntil = validUntil;
    }

    [Column("id")] public int Id { get; init; }

    [Column("discount_code")]
    [MaxLength(50)]
    public string Name { get; init; }

    [Column("discount_type")] public DiscountType DiscountType { get; init; }

    [Column("percentage_or_amount")] public double PercentageOrAmount { get; init; }

    [Column("valid_from")] public DateTime ValidFrom { get; set; }

    [Column("valid_until")] public DateTime ValidUntil { get; set; }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SubscriptionDiscount>()
            .Property(f => f.DiscountType)
            .HasConversion(new DiscountTypeConverter());
    }
}