namespace Orders.Application.DTOs;

public class OrderCreateResponse
{
    public int Id { get; init; }
    public decimal TotalPrice { get; init; }
}