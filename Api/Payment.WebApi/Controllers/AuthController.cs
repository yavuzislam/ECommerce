using Microsoft.AspNetCore.Mvc;
using Payment.BusinessLayer.Abstract;
using Payment.DtoLayer.Dtos.LoginDtos;
using Payment.DtoLayer.Dtos.RegisterDtos;
using Microsoft.AspNetCore.Identity;
using Payment.EntityLayer.Concrete;
using Payment.BusinessLayer.Utilities.Services.MailServices;
using System.Net;

namespace Payment.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSenderService _emailSender;

        public AuthController(IAuthService authService, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IEmailSenderService emailSender)
        {
            _authService = authService;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);

            if (result)
                return Ok("Kayıt başarılı");
            else
                return BadRequest("Kayıt sırasında bir hata oluştu");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {

            var tokenResponse = await _authService.LoginAsync(loginDto);

            if (tokenResponse != null)
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(loginDto.Email);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok(tokenResponse);
            }
            else
                return Unauthorized("Giriş başarısız");
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                await _signInManager.SignOutAsync();
                return Ok("Çıkış başarılı");
            }
            return BadRequest("Kullanıcı bulunamadı");
        }

        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Kullanıcı ID veya token geçersiz.");
            }

            var decodedToken = WebUtility.UrlDecode(token);

            var result = await _authService.ConfirmEmail(userId, decodedToken);

            if (result)
            {
                return Ok("Email doğrulama başarılı.");
            }
            return BadRequest("Email doğrulama başarısız.");
        }
    }
}
