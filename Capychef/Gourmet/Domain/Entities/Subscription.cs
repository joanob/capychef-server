using System.ComponentModel.DataAnnotations.Schema;
using Capychef.Households.Domain.Entities;
using Capychef.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Capychef.Gourmet.Domain.Entities;

[Table("subscriptions")]
public class Subscription
{
    protected Subscription() { }

    [Column("id")] public int Id { get; init; }

    [Column("user_id")] public int UserId { get; init; }

    [Column("household_id")] public int? HouseholdId { get; init; }

    [Column("purchased_at")] public DateTime PurchasedAt { get; init; }

    [Column("valid_from")] public DateTime ValidFrom { get; init; }

    [Column("expires_at")] public DateTime ExpiresAt { get; init; }

    [Column("subscription_type")] public SubscriptionType SubscriptionType { get; init; }

    [Column("automatic_renewal")] public bool AutomaticRenewal { get; set; }

    [Column("is_primary_subscription")] public bool IsPrimarySubscription { get; init; }

    [Column("primary_subscription_id")] public int? PrimarySubscriptionId { get; init; }

    [Column("discount_id")] public int? DiscountId { get; init; }

    [Column("amount_payed")] public double AmountPayed { get; init; }

    [Column("amount_saved")] public double AmountSaved { get; init; }

    [ForeignKey(nameof(UserId))] public User User { get; init; } = null!;

    [ForeignKey(nameof(PrimarySubscriptionId))]
    public Subscription? PrimarySubscription { get; init; }

    [ForeignKey(nameof(HouseholdId))] public Household? Household { get; init; }

    [ForeignKey(nameof(DiscountId))] public SubscriptionDiscount? SubscriptionDiscount { get; init; }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Subscription>()
            .Property(f => f.SubscriptionType)
            .HasConversion(new SubscriptionTypeConverter());
    }
}