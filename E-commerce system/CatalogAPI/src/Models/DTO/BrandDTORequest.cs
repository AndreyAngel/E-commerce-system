using System.ComponentModel.DataAnnotations;

namespace CatalogAPI.Models.DTO;

/// <summary>
/// Brand data transfer object for request
/// </summary>
public class BrandDTORequest
{
    /// <summary>
    /// Brand name
    /// </summary>
    [Required]
    public string? Name { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public string? Description { get; set; }
}
