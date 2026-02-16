using System.Net.Http.Json;
using System.Text.Json;
using Orders.Application.DTOs;

namespace Orders.Application.ExternalServices;

public class ProductCatalogClient :IProductCatalogClient
{
    private readonly HttpClient _client;
    
    public ProductCatalogClient(HttpClient client)
    {
        _client = client;
    }
    
    public async Task<ProductList> GetByIdsAsync(IEnumerable<int> ids)
    {
        var query = string.Join("&", ids.Distinct().Select(id => $"ids={id}"));
        var url = $"api/ProductCatalog/get/productsByIds?{query}";

        try 
        {
            return await _client.GetFromJsonAsync<ProductList>(url);
        }
        catch (Exception ex)
        {
            return new ProductList();
        }
    }

    public async Task<bool> ReleaseStockAsync(IEnumerable<StockUpdate> release)
    {
        var response = await _client.PostAsJsonAsync("api/ProductCatalog/update/release-stock", release);
    
        if (response.IsSuccessStatusCode)
            return true;

        return false;
    }
    
    public async Task<bool> DecreaseStockAsync(IEnumerable<StockUpdate> stocks)
    {
        var response = await _client.PostAsJsonAsync("api/ProductCatalog/update/decrease-stock", stocks);
    
        if (response.IsSuccessStatusCode)
            return true;

        return false;
    }
}