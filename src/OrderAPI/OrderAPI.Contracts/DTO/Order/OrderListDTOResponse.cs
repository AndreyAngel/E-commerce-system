namespace OrderAPI.Contracts.DTO.Order;

/// <summary>
/// Orders list as response
/// </summary>
public class OrderListDTOResponse
{
    /// <summary>
    /// Oredr Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Customer Id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Order is ready?
    /// </summary>
    public bool IsReady { get; set; }

    /// <summary>
    /// Order is received?
    /// </summary>
    public bool IsReceived { get; set; }

    /// <summary>
    /// Oredr is canceled?
    /// </summary>
    public bool IsCanceled { get; set; }

    /// <summary>
    /// Order is paymented?
    /// </summary>
    public bool IsPaymented { get; set; }

    public static DateTime DateTime { get; set; }
}
