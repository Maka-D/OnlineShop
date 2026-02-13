using System.ComponentModel.DataAnnotations;

namespace Orders.Application.DTOs;

public class OrderListRequest
{
    [Required]
    public int UserId { get; set; }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}