using System.ComponentModel.DataAnnotations;
using ProductCatalog.Domain.Models;

namespace ProductCatalog.Application.DTOs;

public class Product
{
   public int Id { get; set; }
   [Required]
   [StringLength(50)]
   public required string Name { get; set; }
   [Required]
   [StringLength(20)]
   public required string SKU { get; set; }
   [Required]
   [Range(0.0, (double)decimal.MaxValue, ErrorMessage = "Price cannot be negative.")]
   public required decimal Price { get; set; }
   [Required]
   [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative!")]
   public required int StockQuantity  { get; set; }
   [Required]
   public required bool IsActive  { get; set; } 
}