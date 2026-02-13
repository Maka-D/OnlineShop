using System.ComponentModel.DataAnnotations;
using ProductCatalog.Domain.Models;

namespace ProductCatalog.Application.DTOs;

public class ProductUpdateRequest
{
    [Required]
    public int Id { get; set; }
    public string SKU { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; } 
}