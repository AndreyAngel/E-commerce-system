namespace OrderAPI.Domain.Entities.Abstractions;

/// <summary>
/// Base database entity
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Entity object Id
    /// </summary>
    public virtual Guid Id { get; set; }
}
