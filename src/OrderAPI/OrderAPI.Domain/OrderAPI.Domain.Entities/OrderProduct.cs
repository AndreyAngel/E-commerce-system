using OrderAPI.Domain.Entities.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderAPI.Domain.Entities;

/// <summary>
/// The order product
/// </summary>
public class OrderProduct : BaseEntity
{
    /// <summary>
    /// The order product Id
    /// Cart product Id = the order product Id
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public override Guid Id { get; set; }

    /// <summary>
    /// Id of the product corresponding ofvthe product from catalog
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Quantity of the order products
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Total value of all order products
    /// </summary>
    public double TotalValue { get; set; }

    /// <summary>
    /// The order Id
    /// </summary>
    public Guid? OrderId { get; set; }

    /// <summary>
    /// The order
    /// </summary>
    public Order? Order { get; set; }
}
