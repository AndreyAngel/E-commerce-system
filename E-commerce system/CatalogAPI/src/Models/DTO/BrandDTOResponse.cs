namespace CatalogAPI.Models.DTO;

/// <summary>
/// Brand data transfer object for response
/// </summary>
public class BrandDTOResponse
{
    /// <summary>
    /// Brand Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Brand name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public string? Description { get; set; }
}
