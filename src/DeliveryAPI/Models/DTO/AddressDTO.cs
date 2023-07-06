using System.ComponentModel.DataAnnotations;

namespace DeliveryAPI.Models.DTO;

/// <summary>
/// Address data transfer object
/// </summary>
public class AddressDTO
{
    /// <summary>
    /// City
    /// </summary>
    [Required]
    public string? City { get; set; }

    /// <summary>
    /// Street
    /// </summary>
    [Required]
    public string? Street { get; set; }

    /// <summary>
    /// Number of home
    /// </summary>
    [Required]
    public string? NumberOfHome { get; set; }

    /// <summary>
    /// Apartment number
    /// </summary>
    public string? ApartmentNumber { get; set; }

    /// <summary>
    /// Postal code
    /// </summary>
    public string? PostalCode { get; set; }
}
