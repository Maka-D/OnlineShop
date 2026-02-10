using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Domain.Models;

namespace ProductCatalog.Infrastructure.EntityMapper;

public abstract class BaseEntityConfiguration<TBase> : IEntityTypeConfiguration<TBase> 
    where TBase : BaseModel
{
    public virtual void Configure(EntityTypeBuilder<TBase> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()"); 

        builder.Property(x => x.UpdatedAt)
            .IsRequired(false);

        builder.Property(x => x.DeletedAt)
            .IsRequired(false);
               
        builder.Property<byte[]>("RowVersion")
            .IsRowVersion()
            .IsConcurrencyToken();
        
        builder.HasQueryFilter(x => x.DeletedAt == null);
    }
}