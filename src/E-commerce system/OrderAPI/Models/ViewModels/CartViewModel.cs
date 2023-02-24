using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderAPI.Models.ViewModels;

public class CartViewModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required(ErrorMessage = "Invalid CartId")]
    public int Id { get; set; } // Generated during user registration (UserId == CartId)

    public List<CartProductViewModelResponse> CartProducts { get; set; } = new List<CartProductViewModelResponse>();


    [Range(0, 9999999999999999999, ErrorMessage = "Invalid total value")]
    public double TotalValue { get; set; } = 0;

    public void ComputeTotalValue()
    {
        TotalValue = CartProducts.Sum(x => x.TotalValue);
    }

    public void Clear()
    {
        CartProducts.Clear();
        TotalValue = 0;
    }
}
