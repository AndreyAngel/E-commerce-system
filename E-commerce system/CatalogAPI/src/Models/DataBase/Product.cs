namespace CatalogAPI.Models.DataBase;

/// <summary>
/// Stores product data
/// </summary>
public class Product : BaseEntity
{
    /// <summary>
    /// Gets or sets a price
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// Gets or sets a category Id
    /// </summary>
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Gets or sets a category
    /// </summary>
    public virtual Category? Category { get; set; }

    /// <summary>
    /// Gets or sets a category Id
    /// </summary>
    public Guid BrandId { get; set; }

    /// <summary>
    /// Gets or sets a brand
    /// </summary>
    public virtual Brand? Brand { get; set; }

    /// <summary>
    /// Gets or sets a stausa producta
    /// true, if is salling
    /// false, if isn't salling
    /// </summary>
    public bool IsSale { get; set; } = false;
}
