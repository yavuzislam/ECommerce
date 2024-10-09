using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Payment.BusinessLayer.Abstract;
using Payment.BusinessLayer.Utilities.Services.MailServices;
using Payment.BusinessLayer.Utilities.Tool;
using Payment.DtoLayer.Dtos.LoginDtos;
using Payment.DtoLayer.Dtos.RegisterDtos;
using Payment.EntityLayer.Concrete;

namespace Payment.BusinessLayer.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthManager> _logger;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly IEmailSenderService _emailSender;

        public AuthManager(
            UserManager<AppUser> userManager,
            IMapper mapper,
            ILogger<AuthManager> logger,
            JwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                var user = _mapper.Map<AppUser>(registerDto);
                user.IsActive = true;

                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Kullanıcı başarıyla kaydedildi. Kullanıcı ID: {UserId}", user.Id);
                    return true;
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogWarning("Kullanıcı kaydı sırasında hata oluştu: {Error}", error.Description);
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı kaydı sırasında bir hata oluştu.");
                throw;
            }
        }

        public async Task<TokenResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    _logger.LogWarning("Kullanıcı bulunamadı. Email: {Email}", loginDto.Email);
                    return null;
                }

                var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

                if (result)
                {
                    _logger.LogInformation("Kullanıcı başarıyla giriş yaptı. Email: {Email}", loginDto.Email);

                    // Kullanıcı rollerini alıyoruz
                    var roles = await _userManager.GetRolesAsync(user);

                    // Kullanıcı bilgilerini hazırlama
                    var userViewModel = new GetCheckAppUserViewModel
                    {
                        ID = user.Id.ToString(),
                        Email = user.Email,
                        Roles = roles.ToList(), // Roller
                        Provider = "Local",
                        IsExist = true
                    };

                    // Token oluşturma
                    var tokenResponse = _jwtTokenGenerator.GenerateToken(userViewModel);

                    return tokenResponse;
                }
                else
                {
                    _logger.LogWarning("Giriş başarısız. Email: {Email}", loginDto.Email);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Giriş işlemi sırasında bir hata oluştu.");
                throw;
            }
        }

        public async Task<bool> ConfirmEmail(string userId, string token)
        {
            try
            {
                if (userId == null || token == null)
                {
                    return false;
                }
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return false;
                }
                var result = await _userManager.ConfirmEmailAsync(user, token);
                return result.Succeeded;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Email onaylama sırasında bir hata oluştu.");
                throw;
            }
        }
    }
}