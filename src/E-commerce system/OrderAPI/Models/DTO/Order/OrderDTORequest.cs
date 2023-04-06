using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models.DTO.Order;

public class OrderDTORequest
{
    [Required]
    public Guid UserId { get; set; }

    public List<OrderCartProductDTORequest> CartProducts { get; set; } = new();
}
