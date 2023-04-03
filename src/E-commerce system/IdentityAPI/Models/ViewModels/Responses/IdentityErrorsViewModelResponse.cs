using Microsoft.AspNetCore.Identity;

namespace OrderAPI.Models.ViewModels.Responses;

/// <summary>
/// The view model of the response containing the identity errors
/// </summary>
public class IdentityErrorsViewModelResponse : IIdentityViewModelResponse
{
    /// <summary>
    /// Gets or sets a identity errors list
    /// </summary>
    public IEnumerable<IdentityError> Errors { get; set; }

    /// <summary>
    /// Creates an instance of the <see cref="IdentityErrorsViewModelResponse"/>.
    /// </summary>
    /// <param name="errors"> Identity errors list </param>
    public IdentityErrorsViewModelResponse(IEnumerable<IdentityError> errors)
    {
        Errors = errors;
    }
}
