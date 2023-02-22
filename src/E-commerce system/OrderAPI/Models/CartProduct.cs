using Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models;

public class CartProduct
{
    public int Id { get; set; }


    [Range(1, 9999999999999999999, ErrorMessage = "Invalid ProductId")]
    public int ProductId { get; set; }
    public Product Product { get; set; }


    [Range(1, 999999, ErrorMessage = "Invalid quantity")]
    public int Quantity { get; set; }


    [Range(0, 9999999999999999999, ErrorMessage = "Invalid total value")]
    public double TotalValue { get; set; }


    [Range(1, 9999999999999999999, ErrorMessage = "Invalid CartId")]
    public int CartId { get; set; }
    public Cart? Cart { get; set; }


    [Range(1, 9999999999999999999, ErrorMessage = "Invalid OrderId")]
    public int? OrderId { get; set; }
    public Order? Order { get; set; }

    public void ComputeTotalValue()
    {
        TotalValue = Quantity * Product.Price;
    }
}
