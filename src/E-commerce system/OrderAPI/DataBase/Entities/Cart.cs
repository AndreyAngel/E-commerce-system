using System.ComponentModel.DataAnnotations.Schema;

namespace OrderAPI.DataBase.Entities;

public class Cart
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; } // Generated during user registration (UserId == CartId)

    public List<CartProduct> CartProducts { get; set; } = new();

    public double TotalValue { get; set; }
}
