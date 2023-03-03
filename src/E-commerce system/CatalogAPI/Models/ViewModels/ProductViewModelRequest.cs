using System.ComponentModel.DataAnnotations;

namespace CatalogAPI.Models.ViewModels;

public class ProductViewModelRequest
{
    [Required]
    public string? Name { get; set; }

    public string? Description { get; set; }

    public double Price { get; set; }

    [Range(1, 9999999999999999999, ErrorMessage = "Invalid CategoryId")]
    public int CategoryId { get; set; }

    [Range(1, 9999999999999999999, ErrorMessage = "Invalid BrandId")]
    public int BrandId { get; set; }
}
