using StoreAPI.DataBase.Entities;

namespace StoreAPI.Models.DTO;

public class StockDTOResponse
{
    public Guid StoreId { get; set; }

    public List<StockProduct> StockProducts { get; set; } = new();
}
