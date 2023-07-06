using StoreAPI.DataBase.Entities;

namespace StoreAPI.Models.DTO;

public class StoreDTOResponse
{
    public Guid Id { get; set; }

    public Address? Address { get; set; }
}
