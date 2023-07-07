using StoreAPI.Domain.Entities;

namespace StoreAPI.UseCases.Interfaces;

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