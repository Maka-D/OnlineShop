using System.ComponentModel.DataAnnotations;

namespace Orders.Application.DTOs;

public class Order
{
    public int Id  { get; set; }
    [Required]
    public string UserId { get; set; } 
    [Required]
    public string Status { get; init; }
    [Required]
    public IEnumerable<OrderItem> Items { get; set; } 
    public decimal TotalPrice { get; init; }
}