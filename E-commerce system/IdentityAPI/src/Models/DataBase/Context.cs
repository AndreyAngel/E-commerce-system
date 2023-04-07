using IdentityAPI.Models.DataBase.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityAPI.Models.DataBase;

/// <summary>
/// Class for the Entity Framework database context used for identity
/// </summary>
public class Context : IdentityDbContext<User>
{
    /// <summary>
    /// Creates an instance of the <see cref="Context"/>.
    /// </summary>
    /// <param name="options"> <see cref="DbContextOptions{Context}"/> </param>
    public Context(DbContextOptions<Context> options) : base(options)
    {
        Database.EnsureCreated();
    }

    /*protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityRole>().HasData(

                new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Role.Admin.ToString(),
                    NormalizedName = Role.Admin.ToString().ToUpper()
                },

                new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Role.Salesman.ToString(),
                    NormalizedName = Role.Salesman.ToString().ToUpper()
                },

                new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Role.Buyer.ToString(),
                    NormalizedName = Role.Buyer.ToString().ToUpper()
                },

                new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Role.Courier.ToString(),
                    NormalizedName = Role.Courier.ToString().ToUpper()
                });
    }*/

    public DbSet<Address> Addresses { get; set; }

    public DbSet<Token> Tokens { get; set; }
}
