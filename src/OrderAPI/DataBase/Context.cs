using Microsoft.EntityFrameworkCore;
using OrderAPI.DataBase.Entities;

namespace OrderAPI.DataBase
{
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

        public DbSet<CartProduct> CartProducts { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderProduct> OrderProducts { get; set; }
    }
}
