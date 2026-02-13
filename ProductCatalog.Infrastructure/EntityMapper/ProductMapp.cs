using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Domain.Models;

namespace ProductCatalog.Infrastructure.EntityMapper;

public class ProductMapp : BaseEntityConfiguration<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);
        
        builder.Property(p => p.Name)
            .HasColumnType("NVARCHAR(50)")             
            .IsRequired();
        
        builder.Property(p => p.SKU)
            .HasColumnType("NVARCHAR(20)")
            .IsRequired();
        
        builder.HasIndex(p => p.SKU).IsUnique();

        builder.ComplexProperty(p => p.Price, b =>
        {
            b.Property(price => price.Value)
                .HasColumnName("Price")
                .HasPrecision(18, 2)
                .IsRequired();
        });

        builder.Property(p => p.StockQuantity)
            .IsRequired();
        
        builder.Property(p => p.IsActive)
            .HasDefaultValue(true)
            .IsRequired();
        
        builder.ToTable(t => t.HasCheckConstraint("CK_Product_Price_Positive", "[Price] >= 0"));
        builder.ToTable(t => t.HasCheckConstraint("CK_Product_StockQuantity_Positive", "[StockQuantity] >= 0"));
    }
}