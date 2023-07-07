namespace StoreAPI.Domain.Entities;

/// <summary>
/// Delivery address
/// </summary>
public class Address
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// City
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// Street
    /// </summary>
    public string Street { get; set; }

    /// <summary>
    /// Number of home
    /// </summary>
    public string NumberOfHome { get; set; }

    public override int GetHashCode()
    {
        return (City + Street + NumberOfHome).GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        return obj is Address address && address.City == City &&
               address.Street == Street && address.NumberOfHome == NumberOfHome;
    }
}
