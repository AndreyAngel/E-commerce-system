namespace OrderAPI.DataBase.Entities;

/// <summary>
/// Order
/// </summary>
public class Order : BaseEntity
{
    /// <summary>
    /// User Id
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// The order products list
    /// </summary>
    public List<OrderProduct> OrderProducts { get; set; } = new();

    /// <summary>
    /// Order is ready?
    /// </summary>
    public bool IsReady { get; set; } = false;

    /// <summary>
    /// Order is received?
    /// </summary>
    public bool IsReceived { get; set; } = false;

    /// <summary>
    /// Order is canceled?
    /// </summary>
    public bool IsCanceled { get; set; } = false;

    /// <summary>
    /// Order is paymented?
    /// </summary>
    public bool IsPaymented { get; set; } = false;

    /// <summary>
    /// Date time of creation of the order
    /// </summary>
    public static DateTime DateTime { get; set; } = DateTime.Now;
}