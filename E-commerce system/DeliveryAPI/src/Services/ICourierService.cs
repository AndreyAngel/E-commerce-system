using DeliveryAPI.DataBase.Entities;

namespace DeliveryAPI.Services;

public interface ICourierService : IDisposable
{
    IEnumerable<Courier> GetAll();

    Courier GetById(Guid Id);

    Task Create(Courier courier);
}
