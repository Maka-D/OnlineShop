using ProductCatalog.Domain.Models;
using Shared.Models;

namespace ProductCatalog.Domain.Interfaces;

public interface IProductCatalogRepository
{
    public Task<IEnumerable<Product>> GetProductsAsync(int pageSize = 10, int pageNumber = 1);
    public Task<Product?> GetProductDetailsAsync(int productId);
    public Task<bool> ExistsBySKU(string sku);
    public Task<Product?> GetBySKU(string sku);
    public void Create(Product product);
    public void Update(Product product);
    public void Delete(int productId);
    Task<IEnumerable<Product>> GetProductsByIdsAsync(IEnumerable<int> ids);
}