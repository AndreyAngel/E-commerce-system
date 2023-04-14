using DeliveryAPI.DataBase.Entities;
using DeliveryAPI.UnitOfWork.Interfaces;

namespace DeliveryAPI.Services;

public class CourierService : ICourierService
{
    public readonly IUnitOfWork _db;
    private bool disposedValue;

    public async Task Create(Courier courier)
    {
        await _db.Couriers.AddAsync(courier);
        await _db.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {

            }

            disposedValue = true;
        }
    }
}
