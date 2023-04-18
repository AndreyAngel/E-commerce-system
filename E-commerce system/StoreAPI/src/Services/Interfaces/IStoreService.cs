using StoreAPI.DataBase.Entities;

namespace StoreAPI.Services.Interfaces;

public interface IStoreService : IDisposable
{
    IEnumerable<Store> GetAll();

    Store GetById(Guid Id);

    StoreProduct GetStoreProductByProductId(Guid storeId, Guid productId);

    IEnumerable<StoreProduct> GetStoreProductsByStoreId(Guid storeId);

    Task<Store> Create(Store store);

    Task<Store> Update(Store store);

    Task Delete(Guid storeId);

    Task TakeProductFromStock(StoreProduct storeProduct);

    Task<StoreProduct> ReturnProductToStock(Guid storeId, Guid storeProductId, int quantity);

    Task SellProduct(Guid storeId, Guid productId);

    Task<StoreProduct> ReturnProductToStore(Guid storeId, Guid ProductId, int quantity);
}