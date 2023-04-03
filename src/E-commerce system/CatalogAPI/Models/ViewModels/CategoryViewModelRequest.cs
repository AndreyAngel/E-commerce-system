using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models.ViewModels;

public class CategoryViewModelRequest
{
    [Required]
    public string? Name { get; set; }
    public string? Description { get; set; }
}
