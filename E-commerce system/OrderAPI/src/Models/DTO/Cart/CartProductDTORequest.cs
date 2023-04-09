using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models.DTO.Cart;

/// <summary>
/// Cart product data transfer object as request
/// </summary>
public class CartProductDTORequest
{
    /// <summary>
    /// Product Id
    /// </summary>
    [Required]
    public Guid ProductId { get; set; }

    /// <summary>
    /// The quantity products in the cart that match a product from the catalog
    /// </summary>
    [Range(1, 1000, ErrorMessage = "Invalid quantity")]
    public int Quantity { get; set; }

    /// <summary>
    /// Cart Id
    /// </summary>
    [Required]
    public Guid CartId { get; set; }
}
