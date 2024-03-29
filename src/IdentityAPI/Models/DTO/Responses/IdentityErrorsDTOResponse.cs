﻿using Microsoft.AspNetCore.Identity;

namespace IdentityAPI.Models.DTO.Response;

/// <summary>
/// The data transfer object of the response containing the identity errors
/// </summary>
public class IdentityErrorsDTOResponse : IDTOResponse
{
    /// <summary>
    /// Identity errors list
    /// </summary>
    public IEnumerable<IdentityError> Errors { get; set; }

    /// <summary>
    /// Creates an instance of the <see cref="IdentityErrorsDTOResponse"/>.
    /// </summary>
    /// <param name="errors"> Identity errors list </param>
    public IdentityErrorsDTOResponse(IEnumerable<IdentityError> errors)
    {
        Errors = errors;
    }
}
