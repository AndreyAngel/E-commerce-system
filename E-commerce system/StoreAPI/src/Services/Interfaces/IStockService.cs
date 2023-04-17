using StoreAPI.DataBase.Entities;

namespace StoreAPI.Services.Interfaces;

public interface IStockService
{
    Stock GetByStoreId(Guid storeId);

    StockProduct GetStockProductByProductId(Guid productId);

    Task<StockProduct> AddProduct(StockProduct product);

    Task<StockProduct> TakeProductFromStock(Guid stockProductId, int quantity);

    Task ReturnProductToStock(StockProduct stockProduct);

    Task WriteOffTheProduct(Guid stockProductId, int quantity);

    Task DeleteProduct(Guid stockProductId);
}