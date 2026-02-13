using Orders.Application.DTOs;

namespace Orders.Application.ExternalServices;

public interface IProductCatalogClient
{
    public Task<ProductList> GetByIdsAsync(IEnumerable<int> id);
    public Task<bool> ReleaseStockAsync(IEnumerable<StockUpdate> release);
    public Task<bool> DecreaseStockAsync(IEnumerable<StockUpdate> stocks);

}