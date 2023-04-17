namespace StoreAPI.DataBase.Entities;

public class Stock
{
    public Guid Id { get; set; }

    public Guid StoreId { get; set; }

    public Store Store { get; set; }

    public List<StockProduct> StockProducts { get; set; }
}