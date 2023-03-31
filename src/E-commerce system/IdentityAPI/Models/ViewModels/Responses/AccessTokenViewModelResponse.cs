namespace IdentityAPI.Models.ViewModels.Responses;

public class AccessTokenViewModelResponse
{
    public int ExpiresIn { get; set; }

    public string AccessToken { get; set; }

    public AccessTokenViewModelResponse(int expiresIn, string accessToken)
    {
        ExpiresIn = expiresIn;
        AccessToken = accessToken;
    }
}
