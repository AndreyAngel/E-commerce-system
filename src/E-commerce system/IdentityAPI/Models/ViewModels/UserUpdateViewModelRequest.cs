namespace IdentityAPI.Models.ViewModels;

public class UserUpdateViewModelRequest
{
    public string? Name { get; set; }

    public string? Surname { get; set; }

    public DateTime? BirthDate { get; set; }

    public AddressViewModel? Address { get; set; }
}
