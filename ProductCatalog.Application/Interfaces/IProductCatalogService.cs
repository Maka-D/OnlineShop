using ProductCatalog.Application.DTOs;
using Shared.Models;

namespace ProductCatalog.Application.Interfaces;

public interface IProductCatalogService
{
   public Task<Result<ProductsListResponse>> GetProductsAsync(ProductsListRequest request);
   public Task<Result<ProductDetailsResponse>> GetProductDetailsAsync(ProductDetailsRequest request);
   public Task<Result<ProductDetailsChunkResponse>> GetProductDetailsByIdsAsync(ProductDetailsChunkRequest request);
   public Task<Result<ProductCreateResponse>> CreateProductAsync(Product product);
   public Task<Result<bool?>> UpdateProductAsync(ProductUpdateRequest product);
   public Task<Result<bool>> DecreaseProductStockAsync(IEnumerable<ProductStockUpdate> request);
   Task<Result<bool>> AdjustStockAsync(IEnumerable<ProductStockUpdate> items);
}