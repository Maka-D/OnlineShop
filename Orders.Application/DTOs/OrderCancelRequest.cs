using System.ComponentModel.DataAnnotations;

namespace Orders.Application.DTOs;

public class OrderCancelRequest
{
    [Required]
    public int OrderId { get; set; }
    [Required]
    public string UserId { get; set; }
}