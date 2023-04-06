using OrderAPI.DataBase.Entities;

namespace OrderAPI.Models;

public class CartDomainModel
{
    public Guid Id { get; set; }

    public List<CartProductDomainModel> CartProducts { get; set; } = new();

    public double TotalValue { get; set; }

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
