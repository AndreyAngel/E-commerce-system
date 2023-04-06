namespace OrderAPI.Models.DTO.Order;

public class OrderListDTOResponse
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public bool IsReady { get; set; }

    public bool IsReceived { get; set; }

    public bool IsCanceled { get; set; }

    public bool IsPaymented { get; set; }

    public static DateTime DateTime { get; set; }
}
