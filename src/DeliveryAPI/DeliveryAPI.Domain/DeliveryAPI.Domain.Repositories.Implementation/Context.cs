using DeliveryAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAPI.Domain.Repositories.Implementation;

/// <summary>
/// Class for the Entity Framework database context
/// </summary>
public class Context : DbContext
{
    /// <summary>
    /// Creates an instance of the <see cref="Context"/>.
    /// </summary>
    /// <param name="options"> <see cref="DbContextOptions{Context}"/> </param>
    public Context(DbContextOptions<Context> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Delivery> Deliveries { get; set; }

    public DbSet<Courier> Couriers { get; set; }
}
