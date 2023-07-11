namespace StoreAPI.Contracts;

public class StoreDTOResponse
{
    public Guid Id { get; set; }

    public AddressDTO? Address { get; set; }
}
