using System.ComponentModel.DataAnnotations;

namespace CatalogAPI.Models.ViewModels;

public class BrandViewModelRequest
{
    [Required]
    public string? Name { get; set; }
    public string? Description { get; set; }
}
