using DeliveryAPI.DataBase.Entities;

namespace DeliveryAPI.Services;

public interface ICourierService : IDisposable
{
    Task Create(Courier courier);
}
