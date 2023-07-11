namespace StoreAPI.Domain.Entities;

public class Store
{
    public Guid Id { get; set; }

    public Guid AddressId { get; set; }

    public Address Address { get; set; }

    public Guid StokeId { get; set; }

    public Stock Stock { get; set; }

    public List<StoreProduct> StoreProducts { get; set; } = new();
}
