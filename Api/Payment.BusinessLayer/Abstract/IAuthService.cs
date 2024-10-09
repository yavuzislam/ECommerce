using Payment.BusinessLayer.Utilities.Tool;
using Payment.DtoLayer.Dtos.LoginDtos;
using Payment.DtoLayer.Dtos.RegisterDtos;

namespace Payment.BusinessLayer.Abstract;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterDto registerDto);
    Task<TokenResponseDto> LoginAsync(LoginDto loginDto);

    Task<bool> ConfirmEmail(string userId, string token);
}
