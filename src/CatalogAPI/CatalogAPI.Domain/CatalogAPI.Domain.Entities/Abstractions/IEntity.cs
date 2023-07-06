namespace CatalogAPI.Domain.Entities.Abstractions;

/// <summary>
/// Base entity interface
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Gets or sets a Id
    /// </summary>
    Guid Id { get; set; }

    /// <summary>
    /// Gets or sets a name
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Gets or sets description
    /// </summary>
    string? Description { get; set; }

    /// <summary>
    /// Gets or sets creation date
    /// </summary>
    DateTime CreationDate { get; set; }
}
