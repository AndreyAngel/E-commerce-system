namespace CatalogAPI.Models.DTO;

/// <summary>
/// Product list data transfer object for response
/// </summary>
public class ProductListDTOResponse
{
    /// <summary>
    /// Product Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Product name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Product price
    /// </summary>
    public double Price { get; set; }
}
