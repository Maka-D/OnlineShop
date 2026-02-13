using System.ComponentModel.DataAnnotations;

namespace Orders.Application.DTOs;

public class OrderItem
{
    public int? OrderId { get; set; }
    [Required]
    public int ProductId { get; init; }
    [Required]
    public string ProductName { get; init; }
    [Required]
    public decimal UnitPrice { get; init; }
    [Required]
    public int Quantity { get; init; }
}