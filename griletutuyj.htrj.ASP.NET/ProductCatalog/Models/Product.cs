using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Brand { get; set; } = string.Empty;

    [Required]
    public string Category { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    public AvailabilityStatus AvailabilityStatus { get; set; } = AvailabilityStatus.InStock;
}
