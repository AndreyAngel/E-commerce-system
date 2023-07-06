using DeliveryAPI.Domain.Entities;
using DeliveryAPI.Domain.Repositories.Interfaces;

namespace DeliveryAPI.Domain.Repositories.Implementation;

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
