using DeliveryAPI.Domain.Entities;
using DeliveryAPI.Domain.Repositories.Interfaces;

namespace DeliveryAPI.Domain.Repositories.Implementation;

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
