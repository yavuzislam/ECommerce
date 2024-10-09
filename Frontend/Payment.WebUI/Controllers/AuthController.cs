using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Payment.EntityLayer.Concrete;
using Payment.WebUI.DTOs.LoginDtos;
using Payment.WebUI.DTOs.RegisterDtos;
using Payment.WebUI.Models;
using Payment.WebUI.Services.AuthServices;
using Payment.WebUI.Services.MailServices;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Text;


namespace Payment.WebUI.Controllers;

public class AuthController : Controller
{
    private readonly IHttpClientFactory _httpclientFactory;
    private readonly IAuthService _authService;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailSenderService _emailSender;

    public AuthController(IHttpClientFactory httpclientFactory, IAuthService authService, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IEmailSenderService emailSender)
    {
        _httpclientFactory = httpclientFactory;
        _authService = authService;
        _signInManager = signInManager;
        _userManager = userManager;
        _emailSender = emailSender;

    }

    [HttpGet]
    public async Task<IActionResult> Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {

        var result = await _authService.RegisterAsync(registerDto);
        if (result)
        {
            var user = await _userManager.FindByEmailAsync(registerDto.Email);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var url = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, token = token }, Request.Scheme);
            var confirmationLink = "<a href='" + url + "'>Click here to confirm your email</a>";
            await _emailSender.SendEmailAsync(user.Email, "Confirm your email", confirmationLink);
            return RedirectToAction("Login");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Kayıt sırasında bir hata oluştu");
            return View(registerDto);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {

        var token = await _authService.LoginAsync(loginDto);
        if (token != null)
        {

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user.TwoFactorEnabled)
            {
                TempData["UserId"] = user.Id;
                TempData["Password"] = loginDto.Password;
                return RedirectToAction("TwoFactorLogin");
            }
            else
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                _authService.SetTokenCookie(token);
                return RedirectToAction("Index", "Home");
            }
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Giriş sırasında bir hata oluştu");
            return View(loginDto);
        }
    }

    [HttpGet]
    public async Task<IActionResult> EnableTwoFactorAuthentication()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return RedirectToAction("Login");

        var key = await _authService.GenerateAuthenticatorKeyAsync(user.Id.ToString());
        if (key != null)
        {
            var authenticatorUri = GenerateQrCodeUri(user.Email, key);

            var model = new EnableTwoFactorAuthenticationViewModel
            {
                SharedKey = FormatKey(key),
                AuthenticatorUri = authenticatorUri
            };
            return View(model);
        }
        return View("Error");
    }

    [HttpPost]
    public async Task<IActionResult> EnableTwoFactorAuthentication(EnableTwoFactorAuthenticationViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return RedirectToAction("Login");

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _authService.VerifyTwoFactorTokenAsync(user.Id.ToString(), model.Code);
        if (result)
        {
            user.TwoFactorEnabled = true;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "İki faktörlü doğrulama başarısız.");
        return View(model);
    }

    [HttpGet]
    public IActionResult TwoFactorLogin()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> TwoFactorLogin(TwoFactorLoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var userId = TempData["UserId"].ToString();
        if (userId == null)
        {
            return RedirectToAction("Login");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return RedirectToAction("Login");
        }

        var authenticatorCode = model.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

        var is2FATokenValid = await _userManager.VerifyTwoFactorTokenAsync(
            user, _userManager.Options.Tokens.AuthenticatorTokenProvider, authenticatorCode);

        if (is2FATokenValid)
        {
            //var loginDto= new LoginDto
            //{
            //    Email = user.Email,
            //    Password = TempData["Password"].ToString()
            //};
            //var token = await _authService.LoginAsync(loginDto);
            //_authService.SetTokenCookie(token);
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ModelState.AddModelError("Code", "Geçersiz iki faktörlü doğrulama kodu.");
            return View();
        }
    }

    private string GenerateQrCodeUri(string email, string unformattedKey)
    {
        const string otpauthUrlFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        return string.Format(
            otpauthUrlFormat,
            UrlEncoder.Default.Encode("PaymentApp"),
            UrlEncoder.Default.Encode(email),
            unformattedKey);
    }

    private string FormatKey(string unformattedKey)
    {
        var result = new StringBuilder();
        int currentPosition = 0;
        while (currentPosition + 4 < unformattedKey.Length)
        {
            result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
            currentPosition += 4;
        }
        if (currentPosition < unformattedKey.Length)
        {
            result.Append(unformattedKey.Substring(currentPosition));
        }
        return result.ToString().ToLowerInvariant();
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
        {
            return RedirectToAction("ErrorPage", new { errorCode = 400 });
        }
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return RedirectToAction("ErrorPage", new { errorCode = 400 });
        }
        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            return RedirectToAction("Login");
        }
        return RedirectToAction("ErrorPage", new { errorCode = 400 });
    }

    [HttpGet]
    public IActionResult AccessDenied(int errorCode = 403)
    {
        var errorViewModel = new ErrorModel
        {
            ErrorCode = errorCode,
            ErrorMessage = errorCode switch
            {
                403 => "Erişim yetkiniz yok! Bu sayfaya erişmek için gerekli yetkiye sahip değilsiniz.",
                404 => "Sayfa bulunamadı! Aradığınız sayfa mevcut değil.",
                500 => "Sunucu hatası! Lütfen daha sonra tekrar deneyin.",
                _ => "Beklenmedik bir hata oluştu."
            }
        };
        return View();
    }

    [HttpGet]
    public IActionResult ErrorPage(int errorCode = 403)
    {
        var errorViewModel = new ErrorModel
        {
            ErrorCode = errorCode,
            ErrorMessage = errorCode switch
            {
                403 => "Erişim yetkiniz yok! Bu sayfaya erişmek için gerekli yetkiye sahip değilsiniz.",
                404 => "Sayfa bulunamadı! Aradığınız sayfa mevcut değil.",
                500 => "Sunucu hatası! Lütfen daha sonra tekrar deneyin.",
                _ => "Beklenmedik bir hata oluştu."
            }
        };
        return View(errorViewModel);
    }

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
        if (user == null)
        {
            return RedirectToAction("ErrorPage", new { errorCode = 400 });
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var url = Url.Action("ResetPassword", "Auth", new { token = token, email = user.Email }, Request.Scheme);

        var resetLink = "<a href='" + url + "'>Click here to reset your password</a>";

        await _emailSender.SendEmailAsync(user.Email, "Reset your password", resetLink);

        return RedirectToAction("Login");
    }

    [HttpGet]
    public async Task<IActionResult> ResetPassword(string token, string email)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
        {
            return RedirectToAction("ErrorPage", new { errorCode = 400 });
        }
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return RedirectToAction("ErrorPage", new { errorCode = 400 });
        }
        var model = new ResetPasswordDto
        {
            Email = email,
            Token = token
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
        if (user == null)
        {
            return RedirectToAction("ErrorPage", new { errorCode = 400 });
        }

        var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);

        if (result.Succeeded)
        {
            return RedirectToAction("ResetPasswordConfirmation");
        }
        return RedirectToAction("ErrorPage", new { errorCode = 400 });
    }

    [HttpGet]
    public async Task<IActionResult> ResetPasswordConfirmation()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        var token = HttpContext.Request.Cookies["jwtToken"];
        if (!string.IsNullOrEmpty(token))
        {
            var client = _httpclientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await client.PostAsync("https://localhost:7066/api/Auth/Logout", null);

            if (responseMessage.IsSuccessStatusCode)
            {
                _authService.RemoveTokenCookie();

                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

                return RedirectToAction("Login");
            }
        }
        _authService.RemoveTokenCookie();
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        return RedirectToAction("Login");
    }
}