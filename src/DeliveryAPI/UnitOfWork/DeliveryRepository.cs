using DeliveryAPI.DataBase;
using DeliveryAPI.DataBase.Entities;
using DeliveryAPI.UnitOfWork.Interfaces;

namespace DeliveryAPI.UnitOfWork;

/// <summary>
/// The delivery repository class containing methods for interaction with the database
/// </summary>
public class DeliveryRepository : GenericRepository<Delivery>, IDeliveryRepository
{
    /// <summary>
    /// Creates an instance of the <see cref="DeliveryRepository"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    public DeliveryRepository(Context context) : base(context)
    { }
}
