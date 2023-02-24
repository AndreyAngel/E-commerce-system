namespace OrderAPI.Models.ViewModels;

public class CartProductViewModelResponse
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public ProductViewModel Product { get; set; }
    public int Quantity { get; set; }
    public double TotalValue { get; set; } = 0;

    public void ComputeTotalValue()
    {
        TotalValue = Quantity * Product.Price;
    }
}
