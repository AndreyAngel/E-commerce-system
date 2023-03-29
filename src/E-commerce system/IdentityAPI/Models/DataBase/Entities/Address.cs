namespace IdentityAPI.Models.DataBase.Entities;

public class Address
{
    public Guid Id { get; set; }

    public string? City { get; set; }

    public string? Street { get; set; }

    public string? NumberOfHome { get; set; }

    public string? ApartmentNumber { get; set; }

    public string? PostalCode { get; set; }
}
