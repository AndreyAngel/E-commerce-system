using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Models.DataBase;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
}
