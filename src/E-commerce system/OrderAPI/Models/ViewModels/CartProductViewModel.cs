using Infrastructure.DTO;
using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models.ViewModels;

public class CartProductViewModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Invalid Id")]
    public int Id { get; set; }


    [Range(1, int.MaxValue, ErrorMessage = "Invalid ProductId")]
    public int ProductId { get; set; }
    public ProductDTO? Product { get; set; }


    [Range(1, 1000, ErrorMessage = "Invalid quantity")]
    public int Quantity { get; set; }


    [Range(0, int.MaxValue, ErrorMessage = "Invalid total value")]
    public double TotalValue { get; set; } = 0;


    [Range(1, int.MaxValue, ErrorMessage = "Invalid CartId")]
    public int CartId { get; set; }

    public void ComputeTotalValue()
    {
        TotalValue = Quantity * Product.Price.Value;
    }
}
