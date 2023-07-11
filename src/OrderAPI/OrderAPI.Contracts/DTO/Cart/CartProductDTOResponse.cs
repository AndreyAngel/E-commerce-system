namespace OrderAPI.Contracts.DTO.Cart;

/// <summary>
/// Cart product data transfer object as response
/// </summary>
public class CartProductDTOResponse
{
    /// <summary>
    /// Cart product Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Product Id from catalog
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Product from catalog
    /// </summary>
    public ProductDTO Product { get; set; }

    /// <summary>
    /// The quantity products in the cart that match a product from the catalog
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// The total value products in the cart that match a product from the catalog
    /// </summary>
    public double TotalValue { get; set; } = 0;
}
