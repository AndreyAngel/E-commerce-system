namespace OrderAPI.DataBase.Entities;

public class CartProduct : BaseEntity
{
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public double TotalValue { get; set; }

    public Guid CartId { get; set; }
    public Cart? Cart { get; set; }

    public Guid? OrderId { get; set; }
    public Order? Order { get; set; }

    public void ComputeTotalValue(double price)
    {
        TotalValue = Quantity * price;
    }
}
