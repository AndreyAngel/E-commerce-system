using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models.DTO.Cart;

public class ProductDTO
{
    [Required]
    public string? Name { get; set; }

    public double Price { get; set; }
}
