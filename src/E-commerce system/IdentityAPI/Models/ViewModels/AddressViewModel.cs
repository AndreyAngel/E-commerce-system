using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models.ViewModels;

/// <summary>
/// Address
/// </summary>
public class AddressViewModel
{
    /// <summary>
    /// Gets or sets a city
    /// </summary>
    [Required]
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
