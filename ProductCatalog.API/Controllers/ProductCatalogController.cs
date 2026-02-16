using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Interfaces;

namespace ProductCatalog.Controllers;

[Authorize]
public class ProductCatalogController :BaseController
{
    private readonly IProductCatalogService _productService;

    public ProductCatalogController(IProductCatalogService productService)
    {
        _productService = productService;
    }
    
    [HttpGet("get/productCatalog")]
    public async Task<IActionResult> GetProductCatalogAsync(int pageSize = 10, int pageNumber = 1)
    {
        var result = await _productService.GetProductsAsync(new ProductsListRequest()
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        });
        return HandleResult(result);
    }

    [HttpGet("get/{productId:int}")]
    public async Task<IActionResult> GetProductDetailsAsync(int productId)
    {
        var result = await _productService.GetProductDetailsAsync(new ProductDetailsRequest()
        {
            ProductId = productId
        });
        return HandleResult(result);
    }
    
    [Authorize(Roles = "User")]
    [HttpGet("get/productsByIds")]
    public async Task<IActionResult> GetProductsByIdsAsync([FromQuery]ProductDetailsChunkRequest productIds)
    {
        var result = await _productService.GetProductDetailsByIdsAsync(productIds);
        return HandleResult(result);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("create/product")]
    public async Task<IActionResult> CreateProduct([FromBody] Product product)
    {
        var result = await _productService.CreateProductAsync(product);
        return HandleResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("update/product")]
    public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateRequest request)
    {
        var result = await _productService.UpdateProductAsync(request);
        return HandleResult(result);
    }

    [Authorize(Roles = "User")]
    [HttpPost("update/decrease-stock")]
    public async Task<IActionResult> DecreaseStock([FromBody] IEnumerable<ProductStockUpdate> request)
    {
        var result = await _productService.DecreaseProductStockAsync(request);
        return HandleResult(result);
    }
    
    [Authorize(Roles = "User")]
    [HttpPost("update/release-stock")]
    public async Task<IActionResult> ReleaseStock([FromBody] IEnumerable<ProductStockUpdate> items)
    {
        var result = await _productService.AdjustStockAsync(items);
        return HandleResult(result);
    }
}