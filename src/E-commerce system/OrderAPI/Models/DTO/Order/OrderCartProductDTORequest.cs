using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models.DTO.Order;

public class OrderCartProductDTORequest
{
    [Required]
    public Guid Id { get; set; }
}
