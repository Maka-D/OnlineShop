using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Orders.Application.DTOs;

public class OrderCreateRequest
{
    [JsonIgnore]
    public string? UserId { get; set; } 
    [Required]
    public List<OrderItem> Items { get; set; } = new();
}