using StoreAPI.DataBase.Entities;

namespace StoreAPI.Services.Interfaces;

public interface IStockService : IDisposable
{
    Stock GetByStoreId(Guid storeId);

    StockProduct GetStockProductByProductId(Guid storeId, Guid productId);

    Task<Stock> Create(Stock stock);

    Task<StockProduct> AddProduct(StockProduct product);

    Task<StockProduct> TakeProductFromStock(Guid storeId, Guid stockProductId, int quantity);

    Task ReturnProductToStock(StockProduct stockProduct);

    Task WriteOffTheProduct(Guid storeId, Guid stockProductId, int quantity);

    Task Delete(Guid storeId);
}