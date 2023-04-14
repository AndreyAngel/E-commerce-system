using DeliveryAPI.DataBase.Entities;
using DeliveryAPI.DataBase;
using DeliveryAPI.UnitOfWork.Interfaces;

namespace DeliveryAPI.UnitOfWork;

/// <summary>
/// The courier repository class containing methods for interaction with the database
/// </summary>
public class CourierRepository : GenericRepository<Courier>, ICourierRepository
{
    /// <summary>
    /// Creates an instance of the <see cref="DeliveryRepository"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    public CourierRepository(Context context) : base(context)
    { }
}
