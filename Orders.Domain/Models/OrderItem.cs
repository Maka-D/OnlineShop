using Orders.Domain.CustomExceptions;
using Shared.Models;

namespace Orders.Domain.Models;

public class OrderItem :BaseModel
{
    public int OrderId { get; set; }
    public int ProductId { get; private set; }
    public string ProductName { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    private OrderItem()
    {
    }

    public OrderItem(int productId, string name, decimal price, int quantity)
    {
        if(price < 0)
            throw new NegativeException("Price cannot be negative");
        if(quantity <= 0)
            throw new NegativeException("Quantity must be greater than zero");
        
        ProductId = productId;
        ProductName = name;
        UnitPrice = price;
        Quantity = quantity;
    }
}