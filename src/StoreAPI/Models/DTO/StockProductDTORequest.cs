using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Models.DTO;

public class StockProductDTORequest
{
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public Guid StoreId { get; set; }
}
