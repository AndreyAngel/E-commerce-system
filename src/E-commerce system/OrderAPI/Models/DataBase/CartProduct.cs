namespace OrderAPI.Models.DataBase;

public class CartProduct : BaseEntity
{
    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public double TotalValue { get; set; }

    public int CartId { get; set; }
    public Cart? Cart { get; set; }

    public int? OrderId { get; set; }
    public Order? Order { get; set; }

    public void ComputeTotalValue(double price)
    {
        TotalValue = Quantity * price;
    }
}
