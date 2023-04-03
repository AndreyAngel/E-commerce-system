namespace OrderAPI.Models.ViewModels.Responses;

/// <summary>
/// The view model of the response containing the access token and refresh token
/// </summary>
public class AuthorizationViewModelResponse : IIdentityViewModelResponse
{
    /// <summary>
    /// Gets or sets a access token lifetime in seconds
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Gets or sets a access token type
    /// </summary>
    public string TokenType { get; set; }

    /// <summary>
    /// Gets or sets a access token
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// Gets or sets a refresh token
    /// </summary>
    public string RefreshToken { get; set; }

    /// <summary>
    /// Creates an instance of the <see cref="AuthorizationViewModelResponse"/>.
    /// </summary>
    /// <param name="expiresIn"> Access token lifetime in seconds </param>
    /// <param name="accessToken"> Access token </param>
    /// <param name="refreshToken"> Refresh token </param>
    /// <param name="tokenType"> Refresh token </param>
    public AuthorizationViewModelResponse(int expiresIn, string accessToken, string refreshToken, string tokenType)
    {
        ExpiresIn = expiresIn;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        TokenType = tokenType;
    }
}
