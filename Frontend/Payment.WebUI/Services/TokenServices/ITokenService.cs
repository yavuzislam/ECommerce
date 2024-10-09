namespace Payment.WebUI.Services.TokenServices;

public interface ITokenService
{
    string GetTokenFromCookies(HttpContext httpContext);
    bool IsTokenValid(string token);
    void AddTokenToRequest(IHttpClientFactory client, string token);
}
