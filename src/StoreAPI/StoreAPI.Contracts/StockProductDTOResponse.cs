namespace StoreAPI.Contracts;

public class StockProductDTOResponse
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public Guid StockId { get; set; }
}
