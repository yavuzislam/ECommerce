using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Payment.EntityLayer.Concrete;
using Payment.WebUI.DTOs.LoginDtos;
using Payment.WebUI.DTOs.RegisterDtos;
using Payment.WebUI.Utilities.Tool;

namespace Payment.WebUI.Services.AuthServices;

public class AuthService : IAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AuthService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<bool> RegisterAsync(RegisterDto registerDto)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync("https://localhost:7066/api/Auth/register", registerDto);

        return response.IsSuccessStatusCode;
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync("https://localhost:7066/api/Auth/login", loginDto);

        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponseDto>(jsonData);
            return tokenResponse.Token; // Token'ı döndürüyoruz
        }

        return null; // Başarısız ise null döndürüyoruz
    }

    // Token'ı cookie'de saklama metodu
    public void SetTokenCookie(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true, // Güvenlik için
            //Expires = DateTime.UtcNow.AddHours(1) // Token'ın süresi ne kadarsa ona göre ayarlayabilirsiniz
            Expires = null, // Tarayıcı kapatılana kadar geçerli olacak
        };

        _httpContextAccessor.HttpContext.Response.Cookies.Append("jwtToken", token, cookieOptions);
    }

    // Token'ı cookie'den kaldırma metodu
    public void RemoveTokenCookie()
    {
        _httpContextAccessor.HttpContext.Response.Cookies.Delete("jwtToken");
    }

    public async Task<bool> ConfirmEmailAsync(string userId, string token)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsync($"https://localhost:7066/api/Auth/ConfirmEmail?userId={userId}&token={token}", null);

        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }





    public async Task<string> GenerateAuthenticatorKeyAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return null;
        }

        await _userManager.ResetAuthenticatorKeyAsync(user);
        var key = await _userManager.GetAuthenticatorKeyAsync(user);

        return key;

    }

    public async Task<bool> VerifyTwoFactorTokenAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, token);
        return isValid;
    }

    public string GetTokenCookie()
    {
        var token = _httpContextAccessor.HttpContext.Request.Cookies["jwtToken"];

        return string.IsNullOrEmpty(token) ? null : token;
    }
}