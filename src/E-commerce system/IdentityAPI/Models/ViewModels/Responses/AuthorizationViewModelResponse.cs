namespace IdentityAPI.Models.ViewModels.Responses;

public class AuthorizationViewModelResponse : IIdentityViewModelResponse
{
    public int ExpiresIn { get; set; }

    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }

    public AuthorizationViewModelResponse(int expiresIn, string accessToken, string refreshToken)
    {
        ExpiresIn = expiresIn;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}
