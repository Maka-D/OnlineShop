using ProductCatalog.Domain.Models;

namespace ProductCatalog.Application.DTOs;

public class ProductDetailsResponse
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string SKU { get; set; }
    public required decimal Price { get; set; }
    public required int StockQuantity  { get; set; }
    public required bool IsActive  { get; set; }
}

public class ProductDetailsChunkResponse
{
    public IEnumerable<ProductDetailsResponse>? Products { get; set; }
}