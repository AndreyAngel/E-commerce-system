namespace StoreAPI.Models.DTO;

public class StoreProductDTOResponse
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public Guid StoreId { get; set; }
}
