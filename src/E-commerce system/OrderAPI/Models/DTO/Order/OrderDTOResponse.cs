using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models.DTO.Order;

public class OrderDTOResponse
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid UserId { get; set; }

    public List<OrderProductDTO> OrderProducts { get; set; } = new();

    public bool IsReady { get; set; }

    public bool IsReceived { get; set; }

    public bool IsCanceled { get; set; }

    public bool IsPaymented { get; set; }
}
