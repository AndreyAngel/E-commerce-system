using System.ComponentModel.DataAnnotations;

namespace CatalogAPI.Contracts.DTO;

/// <summary>
/// Category data transfer object for request
/// </summary>
public class CategoryDTORequest
{
    /// <summary>
    /// Category name
    /// </summary>
    [Required]
    public string? Name { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public string? Description { get; set; }
}
