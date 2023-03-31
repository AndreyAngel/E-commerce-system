namespace IdentityAPI.Models.ViewModels.Responses;

public class UserViewModelResponse
{
    public string Id { get; set; }

    public string Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public DateTime? BirthDate { get; set; }

    public AddressViewModel? Address { get; set; }
}
