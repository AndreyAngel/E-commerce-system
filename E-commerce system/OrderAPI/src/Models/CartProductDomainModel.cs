using OrderAPI.DataBase.Entities;

namespace OrderAPI.Models;

public class CartProductDomainModel
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public ProductDomainModel Product { get; set; }

    public int Quantity { get; set; }

    public double TotalValue { get; set; }

    public Guid CartId { get; set; }
    public Cart? Cart { get; set; }

    public Guid? OrderId { get; set; }
    public Order? Order { get; set; }

    public void ComputeTotalValue()
    {
        TotalValue = Quantity * Product.Price;
    }

    public void ComputeTotalValue(double price)
    {
        TotalValue = Quantity * price;
    }
}
