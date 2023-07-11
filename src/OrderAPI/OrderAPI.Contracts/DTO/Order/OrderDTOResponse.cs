using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Contracts.DTO.Order;

/// <summary>
/// Data transfer object of order as response
/// </summary>
public class OrderDTOResponse
{
    /// <summary>
    /// Order Id
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Customer Id
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Order products list
    /// </summary>
    public List<OrderProductDTO> OrderProducts { get; set; } = new();

    /// <summary>
    /// Order is ready?
    /// </summary>
    public bool IsReady { get; set; }

    /// <summary>
    /// Order is received?
    /// </summary>
    public bool IsReceived { get; set; }

    /// <summary>
    /// Order is canceled?
    /// </summary>
    public bool IsCanceled { get; set; }

    /// <summary>
    /// Order is paymented?
    /// </summary>
    public bool IsPaymented { get; set; }
}
