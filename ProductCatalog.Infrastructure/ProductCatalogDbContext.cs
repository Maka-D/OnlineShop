using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.Models;

namespace ProductCatalog.Infrastructure;

public class ProductCatalogDbContext :DbContext
{
    public ProductCatalogDbContext(DbContextOptions<ProductCatalogDbContext> options) 
        : base(options)
    {
        
    }
    
    public DbSet<Product> Products => Set<Product>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Catalog");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductCatalogDbContext).Assembly);
    }
}