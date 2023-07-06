using System.ComponentModel.DataAnnotations;

namespace CatalogAPI.Contracts.DTO;

/// <summary>
/// Product data transfer object for request
/// </summary>
public class ProductDTORequest
{
    /// <summary>
    /// Product name
    /// </summary>
    [Required]
    public string? Name { get; set; }

    /// <summary>
    /// Descriptions
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Product price
    /// </summary>
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid Price")]
    public double Price { get; set; }

    /// <summary>
    /// Product category Id
    /// </summary>
    [Required]
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Product category Id
    /// </summary>
    [Required]
    public Guid BrandId { get; set; }
}
