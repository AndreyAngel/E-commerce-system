using CatalogAPI.Domain.Entities.Abstractions;

namespace CatalogAPI.Domain.Entities;

/// <summary>
/// Stores category data
/// </summary>
public class Category : BaseEntity
{
    /// <summary>
    /// Gets or sets a product list in this category
    /// </summary>
    public virtual List<Product> Products { get; set; } = new List<Product>();
}
