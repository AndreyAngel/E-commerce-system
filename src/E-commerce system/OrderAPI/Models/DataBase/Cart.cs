namespace OrderAPI.Models.DataBase;

public class Cart : BaseEntity
{
    public List<CartProduct> CartProducts { get; set; } = new List<CartProduct>();

    public double TotalValue { get; set; }

    public void Clear()
    {
        CartProducts.Clear();
        TotalValue = 0;
    }
}
