namespace CatalogAPI.Models.DTO;

/// <summary>
/// Product data transfer object for response
/// </summary>
public class ProductDTOResponse
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
    /// Description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Product price
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// Flag
    /// true, if product is selling
    /// false, if product isn't selling
    /// </summary>
    public bool IsSale { get; set; }

    /// <summary>
    /// Product category
    /// </summary>
    public virtual CategoryDTOResponse? Category { get; set; }

    /// <summary>
    /// Product brand
    /// </summary>
    public virtual BrandDTOResponse? Brand { get; set; }
}
