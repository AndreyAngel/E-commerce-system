using Infrastructure.Models;

namespace OrderAPI.Models;

public class CartProduct
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int Quantity { get; set; }

    public double TotalValue { get; set; }

    public int CartId { get; set; }
    public Cart Cart { get; set; }

    public void ComputeTotalValue()
    {
        TotalValue = Quantity * Product.Price;
    }
}
