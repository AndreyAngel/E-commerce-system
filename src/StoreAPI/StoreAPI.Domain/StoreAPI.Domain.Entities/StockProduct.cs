namespace StoreAPI.Domain.Entities;

public class StockProduct
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public Guid StockId { get; set; }

    public Stock? Stock { get; set; }
}
