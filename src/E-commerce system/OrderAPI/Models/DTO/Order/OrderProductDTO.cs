namespace OrderAPI.Models.DTO.Order;

public class OrderProductDTO
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public double TotalValue { get; set; }
}
