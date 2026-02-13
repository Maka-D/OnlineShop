using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Application.DTOs;

public class ProductStockDecreaseRequest
{
    [Required]
    public int ProductId { get; set; }
    [Required]
    public int Quantity { get; set; }
}