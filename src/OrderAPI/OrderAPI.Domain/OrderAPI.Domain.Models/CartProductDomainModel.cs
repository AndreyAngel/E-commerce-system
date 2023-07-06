namespace OrderAPI.Domain.Models;

/// <summary>
/// Cart product domain model
/// </summary>
public class CartProductDomainModel
{
    /// <summary>
    /// Cart product Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Id of product from catalog
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Product domain model corresponding to the product with catalog
    /// </summary>
    public ProductDomainModel? Product { get; set; }

    /// <summary>
    /// Quantity of all cart products of single type
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Total value of all cart product of single type
    /// </summary>
    public double TotalValue { get; set; }

    /// <summary>
    /// Cart Id
    /// </summary>
    public Guid CartId { get; set; }

    /// <summary>
    /// Cart
    /// </summary>
    public CartDomainModel? Cart { get; set; }

    /// <summary>
    /// Compute total value of all cart porducts of single type
    /// </summary>
    public void ComputeTotalValue()
    {
        TotalValue = Quantity * Product.Price;
    }

    /// <summary>
    /// Compute total value of all cart porducts of single type
    /// </summary>
    /// <param name="price"> Price of the single cart product </param>
    public void ComputeTotalValue(double price)
    {
        TotalValue = Quantity * price;
    }
}
