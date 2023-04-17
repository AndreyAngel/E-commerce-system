using StoreAPI.DataBase.Entities;

namespace StoreAPI.Services.Interfaces;

public interface IStoreService
{
    IEnumerable<Store> GetAll();

    Store GetById(Guid Id);

    StoreProduct GetStoreProductByProductId(Guid productId);

    Task<Store> Create(Store store);

    Task<Store> Update(Store store);

    Task Delete(Guid storeId);

    Task TakeProductFromStock(StoreProduct storeProduct);

    Task<StoreProduct> ReturnProductToStock(Guid storeProductId, int quantity);

    Task SellProduct(Guid storeProductId);

    Task<StoreProduct> ReturnProductToStore(Guid stockProductId, int quantity);

    Task DeleteProduct(Guid storeProductId);
}