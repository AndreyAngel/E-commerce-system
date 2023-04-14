using CatalogAPI.DataBase.Entities;

namespace CatalogAPI.DataBase;

/// <summary>
/// Stores category data
/// </summary>
public class Brand : BaseEntity
{
    /// <summary>
    /// Gets or sets a product list in this category
    /// </summary>
    public virtual List<Product> Products { get; set; } = new List<Product>();
}
