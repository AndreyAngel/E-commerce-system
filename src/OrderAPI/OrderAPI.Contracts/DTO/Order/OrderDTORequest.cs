using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Contracts.DTO.Order;

/// <summary>
/// The order data transfer object as request
/// </summary>
public class OrderDTORequest
{
    /// <summary>
    /// Customer Id
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Cart product IDs for creation the order
    /// </summary>
    public List<OrderCartProductDTORequest> CartProducts { get; set; } = new();

    /// <summary>
    /// flag, true if delivery is needed, false if delivery isn't needed
    /// </summary>
    public bool Delivery { get; set; } = false;

    /// <summary>
    /// Delivery address
    /// </summary>
    public AddressDTO? Address { get; set; } = null;
}
