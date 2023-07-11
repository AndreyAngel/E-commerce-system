namespace OrderAPI.Contracts.DTO.Order;

/// <summary>
/// Orders filters
/// </summary>
public class OrderFilterDTORequest
{
    /// <summary>
    /// Customer Id
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Order is ready?
    /// </summary>
    public bool? IsReady { get; set; }

    /// <summary>
    /// Order is received?
    /// </summary>
    public bool? IsReceived { get; set; }

    /// <summary>
    /// Order is canseled?
    /// </summary>
    public bool? IsCanceled { get; set; }

    /// <summary>
    /// Order is paymented?
    /// </summary>
    public bool? IsPaymented { get; set; }
}
