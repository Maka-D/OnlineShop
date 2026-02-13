using ProductCatalog.Domain.CustomExceptions;
using Shared.Models;

namespace ProductCatalog.Domain.Models;

public class Product :BaseModel
{
    public required string Name { get; set; }
    public required string SKU { get; set; }
    public required Price Price { get; set; }
    public required int StockQuantity  { get; set; }
    public required bool IsActive  { get; set; }
    
    public Result<bool> DecreaseStockQuantity(int quantity)
    {
        if (quantity <= 0)
            return Result<bool>.Failure("Quantity must be greater than zero");
                
        if (!IsActive) 
            return Result<bool>.Failure("Product is not active");

        if (StockQuantity - quantity < 0)
            return Result<bool>.Failure("Not Enough Stock");
    
        StockQuantity -= quantity;
        return Result<bool>.Success(true);
    }
    
    public Result<bool> IncreaseStockQuantity(int quantity)
    {
        if (quantity <= 0)
            return Result<bool>.Failure("Quantity must be greater than zero");
                
        if (!IsActive) 
            return Result<bool>.Failure("Product is not active");
    
        StockQuantity += quantity;
        return Result<bool>.Success(true);
    }
}

public record Price
{
    public decimal Value { get; init; }

    public Price(decimal value)
    {
        if (value < 0) throw new NegativePriceException();
        Value = value;
    }

    public static implicit operator decimal(Price p) => p.Value;
    public static implicit operator Price(decimal v) => new(v);
}