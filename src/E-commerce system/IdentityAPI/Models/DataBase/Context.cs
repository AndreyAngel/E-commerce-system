using OrderAPI.Models.DataBase.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OrderAPI.Models.DataBase;

/// <summary>
/// Class for the Entity Framework database context used for identity
/// </summary>
public class Context : IdentityDbContext<User>
{
    public Context(DbContextOptions<Context> options) : base(options)
    {
        Database.EnsureCreated();
    }
}
