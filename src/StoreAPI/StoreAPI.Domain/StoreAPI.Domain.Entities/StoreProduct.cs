namespace StoreAPI.Domain.Entities;

public class StoreProduct
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public Guid StoreId { get; set; }

    public Store? Store { get; set; }
}
