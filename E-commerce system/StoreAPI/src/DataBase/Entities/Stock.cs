using System.ComponentModel.DataAnnotations.Schema;

namespace StoreAPI.DataBase.Entities;

public class Stock
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    public Guid StoreId { get; set; }

    public Store Store { get; set; }

    public List<StockProduct> StockProducts { get; set; } = new();
}