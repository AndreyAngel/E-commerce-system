using Infrastructure.Exceptions;
using StoreAPI.DataBase.Entities;
using StoreAPI.Services.Interfaces;
using StoreAPI.UnitOfWork.Interfaces;

namespace StoreAPI.Services;

public class StoreService : IStoreService
{
    private readonly IUnitOfWork _db;

    private bool _disposed;

    public StoreService(IUnitOfWork db)
    {
        _db = db;
    }

    public IEnumerable<Store> GetAll()
    {
        return _db.Stores.GetAll();
    }

    public Store GetById(Guid Id)
    {
        var store = _db.Stores.GetById(Id);

        if (store == null)
        {
            throw new NotFoundException("Store with this Id wasn't founded", nameof(Id));
        }

        return store;
    }

    public StoreProduct GetStoreProductByProductId(Guid storeId, Guid productId)
    {
        var storeProduct = _db.StoreProducts.GetAll().SingleOrDefault(x => x.StoreId == storeId
                                                                        && x.ProductId == productId);

        if (storeProduct == null)
        {
            throw new NotFoundException("Store product with this product Id wasn't founded", nameof(productId));
        }

        return storeProduct;
    }

    public IEnumerable<StoreProduct> GetStoreProductsByStoreId(Guid storeId)
    {
        return _db.StoreProducts.GetAll().Where(x => x.StoreId == storeId);
    }

    public async Task<Store> Create(Store store)
    {
        var result = _db.Stores.GetAll().SingleOrDefault(x => x.Equals(store));

        if (result != null)
        {
            throw new ObjectNotUniqueException("The store with this address already exists", nameof(store.Address));
        }

        await _db.Stores.AddAsync(store);
        return store;
    }

    public async Task TakeProductFromStock(StoreProduct storeProduct)
    {
        var result = _db.StoreProducts.GetAll().SingleOrDefault(x => x.StoreId == storeProduct.StoreId
                                                                        && x.ProductId == storeProduct.ProductId);

        if (result == null)
        {
            throw new NotFoundException("Store product with this product Id or store Id wasn't founded");
        }

        result.Quantity += storeProduct.Quantity;
        await _db.StoreProducts.UpdateAsync(result);
    }

    public async Task<StoreProduct> ReturnProductToStock(Guid storeId, Guid storeProductId, int quantity)
    {
        var storeProduct = _db.StoreProducts.GetAll().SingleOrDefault(x => x.StoreId == storeId
                                                                        && x.Id == storeProductId);

        if (storeProduct == null)
        {
            throw new NotFoundException("Store product with this Id or store Id wasn't founded");
        }

        await _db.StoreProducts.RemoveAsync(storeProduct);
        return storeProduct;
    }

    public async Task<StoreProduct> ReturnProductToStore(Guid storeId, Guid productId, int quantity)
    {
        var storeProduct = _db.StoreProducts.GetAll().SingleOrDefault(x => x.StoreId == storeId
                                                                        && x.ProductId == productId);

        if (storeProduct == null)
        {
            throw new NotFoundException("Store product with this product Id or store Id wasn't founded");
        }

        storeProduct.Quantity += quantity;
        await _db.StoreProducts.UpdateAsync(storeProduct);

        return storeProduct;
    }

    public async Task SellProduct(Guid storeId, Guid productId)
    {
        var storeProduct = _db.StoreProducts.GetAll().SingleOrDefault(x => x.StoreId == storeId
                                                                        && x.ProductId == productId);

        if (storeProduct == null)
        {
            throw new NotFoundException("Store product with this product Id or store Id wasn't founded");
        }

        storeProduct.Quantity -= 1;
        await _db.StoreProducts.UpdateAsync(storeProduct);
    }

    public async Task<Store> Update(Store store)
    {
        var result = _db.Stores.GetAll().SingleOrDefault(x => x.Equals(store));

        if (result != null)
        {
            throw new ObjectNotUniqueException("The store with this address already exists", nameof(store.Address));
        }

        await _db.Stores.UpdateAsync(store);
        return store;
    }

    public async Task Delete(Guid storeId)
    {
        var store = _db.Stores.GetById(storeId);

        if (store == null)
        {
            throw new NotFoundException("Store with this Id wasn't founded", nameof(storeId));
        }

        await _db.Stores.RemoveAsync(store);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // TODO: освободить управляемое состояние (управляемые объекты)
            }

            // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить метод завершения
            // TODO: установить значение NULL для больших полей
            _disposed = true;
        }
    }

    // // TODO: переопределить метод завершения, только если "Dispose(bool disposing)" содержит код для освобождения неуправляемых ресурсов
    // ~StoreService()
    // {
    //     // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
