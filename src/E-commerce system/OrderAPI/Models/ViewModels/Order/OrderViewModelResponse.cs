using OrderAPI.Models.ViewModels.Cart;
using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models.ViewModels.Order;

public class OrderViewModelResponse
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid UserId { get; set; }

    public List<CartProductViewModel> CartProducts { get; set; } = new();

    public bool IsReady { get; set; }

    public bool IsReceived { get; set; }

    public bool IsCanceled { get; set; }

    public bool IsPaymented { get; set; }

    public static DateTime DateTime { get; set; }
}
