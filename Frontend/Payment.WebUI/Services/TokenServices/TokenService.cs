
using System.Net.Http.Headers;

namespace Payment.WebUI.Services.TokenServices;

public class TokenService : ITokenService
{
    public void AddTokenToRequest(IHttpClientFactory client, string token)
    {
        if (IsTokenValid(token))
        {
            client.CreateClient().DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public string GetTokenFromCookies(HttpContext httpContext)
    {
        return httpContext.Request.Cookies["jwtToken"];
    }

    public bool IsTokenValid(string token)
    {
        return !string.IsNullOrEmpty(token);
    }
}
