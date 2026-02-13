using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Domain.Models;

namespace Orders.Infrastructure.EntityMapper;

public class OrderMap :BaseEntityConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        base.Configure(builder);

        builder.HasIndex(o => o.IdempotencyKey).IsUnique();

        builder.Property(o => o.Status)
            .HasConversion<string>()
            .IsRequired();
        
        builder.Property(o => o.TotalPrice)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2);

        builder.Metadata.FindNavigation(nameof(Order.Items))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey("OrderId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}