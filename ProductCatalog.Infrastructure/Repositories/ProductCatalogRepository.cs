using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.Interfaces;
using ProductCatalog.Domain.Models;
using Shared.Models;

namespace ProductCatalog.Infrastructure.Repositories;

public class ProductCatalogRepository :IProductCatalogRepository
{
    private readonly ProductCatalogDbContext _dbContext;

    public ProductCatalogRepository(ProductCatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<Product>> GetProductsAsync(int pageSize, int pageNumber)
    {
        return await _dbContext.Products
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Product?> GetProductDetailsAsync(int productId)
    {
        return await _dbContext.Products.FindAsync(productId);
    }

    public async Task<bool> ExistsBySKU(string sku)
    {
        return await _dbContext.Products.AnyAsync(p => p.SKU == sku);
    }

    public async Task<Product?> GetBySKU(string sku)
    {
        return await _dbContext.Products.SingleAsync(x => x.SKU == sku);
    }

    public void Create(Product product)
    {
        _dbContext.Add(product);
    }

    public void Update(Product product)
    {
        _dbContext.Update(product);
    }
    
    public async Task<IEnumerable<Product>> GetProductsByIdsAsync(IEnumerable<int> ids)
    {
        if (ids == null || !ids.Any())
        {
            return Enumerable.Empty<Product>();
        }

        return await _dbContext.Products
            .Where(p => ids.Contains(p.Id))
            .ToListAsync();
    }
}