using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Contracts.DTO.Order;

/// <summary>
/// Stores the product Id from the cart for the order creation request
/// </summary>
public class OrderCartProductDTORequest
{
    /// <summary>
    /// Cart product id
    /// </summary>
    [Required]
    public Guid Id { get; set; }
}
