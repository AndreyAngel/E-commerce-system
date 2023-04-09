using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models.DTO.Cart;

/// <summary>
/// Product data transfer object
/// </summary>
public class ProductDTO
{
    /// <summary>
    /// Product name
    /// </summary>
    [Required]
    public string? Name { get; set; }

    /// <summary>
    /// Product price
    /// </summary>
    public double Price { get; set; }
}
