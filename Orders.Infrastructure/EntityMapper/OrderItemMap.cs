using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Domain.Models;

namespace Orders.Infrastructure.EntityMapper;

public class OrderItemMap :BaseEntityConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        base.Configure(builder);

        builder.Property(oi => oi.ProductName)
            .IsRequired();

        builder.Property(oi => oi.UnitPrice)
            .HasColumnType("decimal(18,2)")
            .HasPrecision(18, 2) 
            .IsRequired();

        builder.Property(oi => oi.Quantity)
            .IsRequired();
        
        builder.Property(oi => oi.ProductId)
            .IsRequired();
    }
}