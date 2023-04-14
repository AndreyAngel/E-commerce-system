namespace Infrastructure.DTO;

public class AddressDTORabbitMQ
{
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

    /// <summary>
    /// Apartment number
    /// </summary>
    public string? ApartmentNumber { get; set; }

    /// <summary>
    /// Postal code
    /// </summary>
    public string? PostalCode { get; set; }
}
