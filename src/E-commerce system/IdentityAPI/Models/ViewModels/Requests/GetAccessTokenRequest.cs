using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Models.ViewModels.Requests;

public class GetAccessTokenRequest
{
    [Required]
    public string? RefreshToken { get; set; }
}
