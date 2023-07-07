namespace StoreAPI.Contracts;

/// <summary>
/// Delivery address
/// </summary>
public class AddressDTO
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Region
    /// </summary>
    public string Region { get; set; }

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
}
