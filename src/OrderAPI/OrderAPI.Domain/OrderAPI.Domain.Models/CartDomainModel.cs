namespace OrderAPI.Domain.Models;

/// <summary>
/// Cart domain model
/// </summary>
public class CartDomainModel
{
    /// <summary>
    /// Cart Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Cart products list
    /// </summary>
    public List<CartProductDomainModel> CartProducts { get; set; } = new();

    /// <summary>
    /// Total value of all cart products
    /// </summary>
    public double TotalValue { get; set; }

    /// <summary>
    /// Compute total value of all cart products
    /// </summary>
    public void ComputeTotalValue()
    {
        TotalValue = CartProducts.Sum(x => x.TotalValue);
    }

    /// <summary>
    /// Clear cart
    /// </summary>
    public void Clear()
    {
        CartProducts.Clear();
        TotalValue = 0;
    }
}
