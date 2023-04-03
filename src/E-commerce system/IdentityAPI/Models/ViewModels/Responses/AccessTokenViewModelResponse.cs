namespace OrderAPI.Models.ViewModels.Responses;

/// <summary>
/// The view model of the response containing the access token
/// </summary>
public class AccessTokenViewModelResponse
{
    /// <summary>
    /// Gets or sets a access token lifetime in seconds
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Gets or sets a access token
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// Gets or sets a access token type
    /// </summary>
    public string TokenType { get; set; }

    /// <summary>
    /// Creates an instance of the <see cref="AccessTokenViewModelResponse"/>.
    /// </summary>
    /// <param name="expiresIn"> Access token lifetime in seconds </param>
    /// <param name="accessToken"> Access token </param>
    /// <param name="tokenType"> Access token type </param>
    public AccessTokenViewModelResponse(int expiresIn, string accessToken, string tokenType)
    {
        ExpiresIn = expiresIn;
        AccessToken = accessToken;
        TokenType = tokenType;
    }
}
