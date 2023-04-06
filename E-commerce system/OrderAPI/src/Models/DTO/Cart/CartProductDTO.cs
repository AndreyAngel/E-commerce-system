using Infrastructure.DTO;
using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models.DTO.Cart;

public class CartProductDTO
{
    [Required]
    public Guid Id { get; set; }


    [Required]
    public Guid ProductId { get; set; }
    public ProductDTORabbitMQ? Product { get; set; }


    [Range(1, 1000, ErrorMessage = "Invalid quantity")]
    public int Quantity { get; set; }


    [Range(0, int.MaxValue, ErrorMessage = "Invalid total value")]
    public double TotalValue { get; set; } = 0;


    [Required]
    public Guid CartId { get; set; }

    public void ComputeTotalValue()
    {
        TotalValue = Quantity * Product.Price.Value;
    }
}
