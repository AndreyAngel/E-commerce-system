namespace OrderAPI.Models.ViewModels.Cart;

public class CartProductViewModelResponse
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public ProductViewModel Product { get; set; }

    public int Quantity { get; set; }

    public double TotalValue { get; set; } = 0;

    public void ComputeTotalValue()
    {
        TotalValue = Quantity * Product.Price;
    }
}
