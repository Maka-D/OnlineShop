namespace Orders.Application.DTOs;

public class OrderListResponse
{
    public IEnumerable<Order>? Orders { get; set; }
}