namespace CatalogAPI.Models.DTO;

/// <summary>
/// Product filter data transfer object
/// </summary>
public class ProductFilterDTO
{
    /// <summary>
    /// Product category Id
    /// </summary>
    public Guid? CategoryId { get; set; }

    /// <summary>
    /// Product brand Id
    /// </summary>
    public Guid? BrandId { get; set; }
}
