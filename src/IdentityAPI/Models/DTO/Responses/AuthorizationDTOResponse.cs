﻿namespace IdentityAPI.Models.DTO.Response;

/// <summary>
/// The data transfer object of the response containing the access token and refresh token
/// </summary>
public class AuthorizationDTOResponse : IDTOResponse
{
    /// <summary>
    /// User Id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Access token lifetime in seconds
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Access token type
    /// </summary>
    public string TokenType { get; set; }

    /// <summary>
    /// Access token
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// Refresh token
    /// </summary>
    public string RefreshToken { get; set; }

    /// <summary>
    /// Creates an instance of the <see cref="AuthorizationDTOResponse"/>.
    /// </summary>
    /// <param name="expiresIn"> Access token lifetime in seconds </param>
    /// <param name="accessToken"> Access token </param>
    /// <param name="refreshToken"> Refresh token </param>
    /// <param name="tokenType"> Access token type </param>
    /// <param name="userId"> userId </param>
    public AuthorizationDTOResponse(int expiresIn,
                                    string accessToken,
                                    string refreshToken,
                                    string tokenType,
                                    Guid userId)
    {
        ExpiresIn = expiresIn;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        TokenType = tokenType;
        UserId = userId;
    }
}
