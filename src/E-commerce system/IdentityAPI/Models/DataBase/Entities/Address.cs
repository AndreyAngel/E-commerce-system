namespace IdentityAPI.Models.DataBase.Entities;

/// <summary>
/// Entity storing address data
/// </summary>
public class Address
{
    /// <summary>
    /// Gets or sets a Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or set a city
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// Gets or sets a street
    /// </summary>
    public string? Street { get; set; }

    /// <summary>
    /// Gets or sets a number of home
    /// </summary>
    public string? NumberOfHome { get; set; }

    /// <summary>
    /// Gets or sets a apartment number
    /// </summary>
    public string? ApartmentNumber { get; set; }

    /// <summary>
    /// Gets or sets a postal code
    /// </summary>
    public string? PostalCode { get; set; }
}
