namespace OrderAPI.Models.DataBase;

public class Order : BaseEntity
{
    public Guid UserId { get; set; }

    public List<CartProduct> CartProducts { get; set; } = new();

    public bool IsReady { get; set; } = false;

    public bool IsReceived { get; set; } = false;

    public bool IsCanceled { get; set; } = false;

    public bool IsPaymented { get; set; } = false;

    public static DateTime DateTime { get; set; } = DateTime.Now;
}