using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models.ViewModels.Order;

public class OrderCartProductViewModelRequest
{
    [Required]
    public Guid Id { get; set; }
}
