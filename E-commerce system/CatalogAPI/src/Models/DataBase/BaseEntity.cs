namespace CatalogAPI.Models.DataBase;

/// <summary>
/// Stores base entity data
/// </summary>
public class BaseEntity : IEntity
{
    /// <summary>
    /// Gets or sets a Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets a name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets a descriptions
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a creation date
    /// </summary>
    public DateTime CreationDate { get; set; } = DateTime.Now;
}
