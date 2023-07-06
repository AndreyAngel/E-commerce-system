using StoreAPI.DataBase.Entities;
using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Models.DTO;

public class StoreDTORequest
{
    [Required]
    public Address? Address { get; set; }
}
