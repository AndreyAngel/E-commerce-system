using System.ComponentModel.DataAnnotations;

namespace CatalogAPI.Models.DTO;

public class CategoryDTORequest
{
    [Required]
    public string? Name { get; set; }

    public string? Description { get; set; }
}
