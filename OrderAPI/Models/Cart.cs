using System.ComponentModel.DataAnnotations.Schema;

namespace OrderAPI.Models;

public class Cart
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; } // Generated during user registration

    public List<CartProduct> cartProducts { get; set; } = new List<CartProduct>();

    private double _TotalValue { get; set; } = 0;

    public void ComputeTotalValue()
    {
        _TotalValue = cartProducts.Sum(x => x.TotalValue);
    }

    public void Clear()
    {
        cartProducts.Clear();
        _TotalValue = 0;
    }

    public double GetTotalValue()
    {
        return _TotalValue;
    }
}
