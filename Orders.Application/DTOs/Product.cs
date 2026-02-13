namespace Orders.Application.DTOs;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string SKU { get; set; }
    public required decimal Price { get; set; }
    public required int StockQuantity  { get; set; }
    public required bool IsActive  { get; set; } 
}

public class ProductList
{
    public IEnumerable<Product?> Products { get; set; }
}