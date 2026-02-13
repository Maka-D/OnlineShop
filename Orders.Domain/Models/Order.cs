using Orders.Domain.CustomExceptions;
using Orders.Domain.Enums;
using Shared.Models;

namespace Orders.Domain.Models;

public class Order :BaseModel
{
    public Guid IdempotencyKey { get; set; }
    public string UserId { get; private set; } 
    public OrderStatus Status { get; private set; }
    public decimal TotalPrice {get; private set;}
    
    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    
    private Order() { }

    public Order(string userId)
    {
        UserId = userId;
        Status = OrderStatus.Pending;
    }

    public void AddItem(int productId, string name, decimal priceAtPurchase, int quantity)
    {
        _items.Add(new OrderItem(productId, name, priceAtPurchase, quantity));
        TotalPrice += quantity * priceAtPurchase;
    }
    
    public void Cancel()
    {
        if (Status != OrderStatus.Pending)
            throw new OrderCancelationFailedException();
        
        Status = OrderStatus.Cancelled;
    }
    
    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new OrderConfirmationFailedException();
        
        Status = OrderStatus.Confirmed;
    }
    
    public void Reject()
    {
        if (Status != OrderStatus.Pending)
            throw new OrderConfirmationFailedException();
        
        Status = OrderStatus.Rejected;
    }
}