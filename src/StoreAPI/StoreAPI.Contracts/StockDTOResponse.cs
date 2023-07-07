namespace StoreAPI.Contracts;

public class StockDTOResponse
{
    public Guid StoreId { get; set; }

    public List<StockProductDTOResponse> StockProducts { get; set; } = new();
}
