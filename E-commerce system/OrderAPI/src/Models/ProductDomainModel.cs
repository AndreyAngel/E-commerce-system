namespace OrderAPI.Models;

/// <summary>
/// Product domain model corresponding of the product from catalog
/// </summary>
public class ProductDomainModel
{
    /// <summary>
    /// Name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Price
    /// </summary>
    public double Price { get; set; }
}
