using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Contracts;

public class StockProductDTORequest
{
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public Guid StoreId { get; set; }
}
