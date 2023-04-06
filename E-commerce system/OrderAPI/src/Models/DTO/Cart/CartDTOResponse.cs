namespace OrderAPI.Models.DTO.Cart;

public class CartDTOResponse
{
    public Guid Id { get; set; }

    public List<CartProductDTOResponse> CartProducts { get; set; } = new();

    public double TotalValue { get; set; } = 0;
}
