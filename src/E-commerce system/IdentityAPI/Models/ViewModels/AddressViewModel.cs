using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Models.ViewModels;

public class AddressViewModel
{
    [Required]
    public string? City { get; set; }

    public string? Street { get; set; }

    public string? NumberOfHome { get; set; }

    public string? ApartmentNumber { get; set; }

    public string? PostalCode { get; set; }
}
