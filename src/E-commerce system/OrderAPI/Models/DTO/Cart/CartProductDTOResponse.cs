namespace OrderAPI.Models.DTO.Cart;

public class CartProductDTOResponse
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public ProductDTO Product { get; set; }

    public int Quantity { get; set; }

    public double TotalValue { get; set; } = 0;

    public void ComputeTotalValue()
    {
        TotalValue = Quantity * Product.Price;
    }
}
