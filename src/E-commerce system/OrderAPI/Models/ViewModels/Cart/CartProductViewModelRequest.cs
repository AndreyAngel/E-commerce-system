using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models.ViewModels.Cart;

public class CartProductViewModelRequest
{
    [Required]
    public Guid ProductId { get; set; }


    [Range(1, 1000, ErrorMessage = "Invalid quantity")]
    public int Quantity { get; set; }


    [Required]
    public Guid CartId { get; set; }
}
