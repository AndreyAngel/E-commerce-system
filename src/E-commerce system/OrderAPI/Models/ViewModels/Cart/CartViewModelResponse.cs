namespace OrderAPI.Models.ViewModels.Cart;

public class CartViewModelResponse
{
    public Guid Id { get; set; }

    public List<CartProductViewModelResponse> CartProducts { get; set; } = new List<CartProductViewModelResponse>();

    public double TotalValue { get; set; } = 0;

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
