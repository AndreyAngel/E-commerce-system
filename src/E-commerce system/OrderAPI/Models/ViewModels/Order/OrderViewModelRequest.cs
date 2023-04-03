using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models.ViewModels.Order;

public class OrderViewModelRequest
{
    [Required]
    public Guid UserId { get; set; }

    public List<OrderCartProductViewModelRequest> CartProducts { get; set; } = new();
}
