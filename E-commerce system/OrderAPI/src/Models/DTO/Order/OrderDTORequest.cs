using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models.DTO.Order;

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
}
