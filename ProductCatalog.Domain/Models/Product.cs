using ProductCatalog.Domain.CustomExceptions;

namespace ProductCatalog.Domain.Models;

public class Product :BaseModel
{
    public required string Name { get; set; }
    public required string SKU { get; set; }
    public required Price Price { get; set; }
    public required int StockQuantity  { get; set; }
    public required bool IsActive  { get; set; }
    
    public void DecreaseStockQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new NegativeStockQuantityException();
                
        if (!IsActive) 
            throw new InactiveProductStockDecreaseException();

        if (StockQuantity - quantity < 0)
            throw new NotEnoughStockAmountException();
    
        StockQuantity -= quantity;
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