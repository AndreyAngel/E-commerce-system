using System.ComponentModel.DataAnnotations.Schema;

namespace OrderAPI.DataBase.Entities;

/// <summary>
/// Cart
/// </summary>
public class Cart
{
    /// <summary>
    /// Cart Id
    /// Generated during user registration (UserId == CartId)
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    /// <summary>
    /// Cart products list
    /// </summary>
    public List<CartProduct> CartProducts { get; set; } = new();

    /// <summary>
    /// Total value of all cart products
    /// </summary>
    public double TotalValue { get; set; }
}
