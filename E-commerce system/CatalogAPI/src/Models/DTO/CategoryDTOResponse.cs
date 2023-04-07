namespace CatalogAPI.Models.DTO;

/// <summary>
/// Category data transfer object for response
/// </summary>
public class CategoryDTOResponse
{
    /// <summary>
    /// Category Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Category name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public string? Description { get; set; }
}
