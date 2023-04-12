using IdentityAPI.DataBase.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityAPI.DataBase;

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

    public DbSet<Address> Addresses { get; set; }

    public DbSet<Token> Tokens { get; set; }
}
