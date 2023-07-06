using Infrastructure.Exceptions;
using StoreAPI.DataBase.Entities;
using StoreAPI.Services.Interfaces;
using StoreAPI.UnitOfWork.Interfaces;

namespace StoreAPI.Services;

public class StockService : IStockService
{
    private readonly IUnitOfWork _db;

    private bool disposedValue;

    public StockService(IUnitOfWork db)
    {
        _db = db;
    }

    public Stock GetByStoreId(Guid storeId)
    {
        var stock = _db.Stocks.Include(x => x.StockProducts).SingleOrDefault(x => x.StoreId == storeId);

        if (stock == null)
        {
            throw new NotFoundException("Stock with this storeId wasn't founded");
        }
        
        return stock;
    }

    public StockProduct GetStockProductByProductId(Guid storeId, Guid productId)
    {
        var stockProduct = _db.StockProducts.GetAll().SingleOrDefault(x => x.ProductId == productId
                                                                        && x.StockId == storeId);

        if (stockProduct == null)
        {
            throw new NotFoundException("Stock product with this product Id or stock Id wasn't founded");
        }

        return stockProduct;
    }

    public async Task<Stock> Create(Stock stock)
    {
        if (_db.Stocks.Include(x => x.StockProducts).SingleOrDefault(x => x.StoreId == stock.StoreId) == null)
        {
            throw new ObjectNotUniqueException("The stock with this store Id already exists", nameof(stock.StoreId));
        }

        await _db.Stocks.AddAsync(stock);
        return stock;
    }

    public async Task<StockProduct> AddProduct(StockProduct product)
    {
        var stockProduct = _db.StockProducts.GetAll().SingleOrDefault(x => x.ProductId == product.ProductId
                                                                        && x.StockId == product.StockId);

        if (stockProduct == null)
        {
            throw new NotFoundException("Stock product with this product Id or stock Id wasn't founded");
        }

        stockProduct.Quantity += product.Quantity;
        await _db.StockProducts.UpdateAsync(stockProduct);

        return stockProduct;
    }
     
    public async Task ReturnProductToStock(StockProduct product)
    {
        var stockProduct = _db.StockProducts.GetAll().SingleOrDefault(x => x.ProductId == product.ProductId
                                                                        && x.StockId == product.StockId);

        if (stockProduct == null)
        {
            throw new NotFoundException("Stock product with this product Id or stock Id wasn't founded");
        }

        stockProduct.Quantity += product.Quantity;
        await _db.StockProducts.UpdateAsync(stockProduct);
    }

    public async Task<StockProduct> TakeProductFromStock(Guid storeId, Guid stockProductId, int quantity)
    {
        var stockProduct = _db.StockProducts.GetAll().SingleOrDefault(x => x.Id == stockProductId
                                                                        && x.StockId == storeId);

        if (stockProduct == null)
        {
            throw new NotFoundException("Stock product with this Id or stock Id wasn't founded");
        }

        stockProduct.Quantity -= quantity;
        await _db.StockProducts.UpdateAsync(stockProduct);

        return stockProduct;
    }

    public async Task WriteOffTheProduct(Guid storeId, Guid stockProductId, int quantity)
    {
        var stockProduct = _db.StockProducts.GetAll().SingleOrDefault(x => x.Id == stockProductId
                                                                        && x.StockId == storeId);

        if (stockProduct == null)
        {
            throw new NotFoundException("Stock product with this Id or stock Id wasn't founded");
        }

        stockProduct.Quantity -= quantity;
        await _db.StockProducts.UpdateAsync(stockProduct);
    }

    public async Task Delete(Guid storeId)
    {
        var stock = _db.Stocks.Include(x => x.StockProducts).SingleOrDefault(x => x.StoreId == storeId);

        if (stock == null)
        {
            throw new NotFoundException("Stock with this storeId wasn't founded");
        }

        await _db.Stocks.RemoveAsync(stock);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: освободить управляемое состояние (управляемые объекты)
            }

            // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить метод завершения
            // TODO: установить значение NULL для больших полей
            disposedValue = true;
        }
    }

    // // TODO: переопределить метод завершения, только если "Dispose(bool disposing)" содержит код для освобождения неуправляемых ресурсов
    // ~StockService()
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
