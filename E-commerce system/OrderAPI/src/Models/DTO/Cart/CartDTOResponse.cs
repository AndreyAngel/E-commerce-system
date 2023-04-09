namespace OrderAPI.Models.DTO.Cart;

/// <summary>
/// Cart data transfer object as response
/// </summary>
public class CartDTOResponse
{
    /// <summary>
    /// Cart Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Cart products
    /// </summary>
    public List<CartProductDTOResponse> CartProducts { get; set; } = new();

    /// <summary>
    /// Total value products in cart
    /// </summary>
    public double TotalValue { get; set; } = 0;
}
