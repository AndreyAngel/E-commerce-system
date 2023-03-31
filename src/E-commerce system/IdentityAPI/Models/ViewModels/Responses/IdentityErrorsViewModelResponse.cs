using Microsoft.AspNetCore.Identity;

namespace IdentityAPI.Models.ViewModels.Responses;

public class IdentityErrorsViewModelResponse : IIdentityViewModelResponse
{
    public IEnumerable<IdentityError> Errors { get; set; }

    public IdentityErrorsViewModelResponse(IEnumerable<IdentityError> errors)
    {
        Errors = errors;
    }
}
