namespace OrderAPI.Models.ViewModels.Order;

public class OrderListViewModelResponse
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public bool IsReady { get; set; }

    public bool IsReceived { get; set; }

    public bool IsCanceled { get; set; }

    public bool IsPaymented { get; set; }

    public static DateTime DateTime { get; set; }
}
