using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models.ViewModels.Cart;

public class ProductViewModel
{
    [Required]
    public string? Name { get; set; }

    public double Price { get; set; }
}
