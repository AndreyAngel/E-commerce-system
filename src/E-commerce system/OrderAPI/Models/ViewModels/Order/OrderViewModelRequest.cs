namespace OrderAPI.Models.ViewModels.Order;

public class OrderViewModelRequest
{
    public int UserId { get; set; }

    public List<OrderCartProductViewModelRequest> CartProducts { get; set; } = new();
}
