using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models.ViewModels.Cart;

public class CartProductViewModelRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Invalid ProductId")]
    public int ProductId { get; set; }


    [Range(1, 1000, ErrorMessage = "Invalid quantity")]
    public int Quantity { get; set; }


    [Range(1, int.MaxValue, ErrorMessage = "Invalid CartId")]
    public int CartId { get; set; }
}
