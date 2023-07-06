using OrderAPI.Domain.Entities.Abstractions;

namespace OrderAPI.Domain.Entities;

/// <summary>
/// Cart product
/// </summary>
public class CartProduct : BaseEntity
{
    /// <summary>
    /// Product Id
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Total value of all cart products of single type
    /// </summary>
    public double TotalValue { get; set; }

    /// <summary>
    /// Cart Id
    /// </summary>
    public Guid CartId { get; set; }

    /// <summary>
    /// Cart
    /// </summary>
    public Cart? Cart { get; set; }
}
