using Payment.WebUI.DTOs.LoginDtos;
using Payment.WebUI.DTOs.RegisterDtos;

namespace Payment.WebUI.Services.AuthServices;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterDto registerDto);
    Task<string> LoginAsync(LoginDto loginDto);
    void SetTokenCookie(string token);
    void RemoveTokenCookie();
    Task<bool> ConfirmEmailAsync(string userId, string token);

    string GetTokenCookie();

    Task<string> GenerateAuthenticatorKeyAsync(string userId);
    Task<bool> VerifyTwoFactorTokenAsync(string userId, string token);

}
