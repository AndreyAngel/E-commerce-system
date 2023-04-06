namespace OrderAPI.Models.DTO.Cart;

public class CartDTOResponse
{
    public Guid Id { get; set; }

    public List<CartProductDTOResponse> CartProducts { get; set; } = new List<CartProductDTOResponse>();

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
