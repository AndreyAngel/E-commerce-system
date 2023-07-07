using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Contracts;

public class StoreDTORequest
{
    [Required]
    public AddressDTO? Address { get; set; }
}
