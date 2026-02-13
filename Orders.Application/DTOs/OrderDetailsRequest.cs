using System.ComponentModel.DataAnnotations;

namespace Orders.Application.DTOs;

public class OrderDetailsRequest
{
    [Required]
    public int OrderId { get; set; }
}