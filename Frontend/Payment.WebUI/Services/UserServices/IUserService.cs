using Payment.WebUI.DTOs.AppUserDtos;

namespace Payment.WebUI.Services.UserServices;

public interface IUserService
{
    Task<List<ResultAppUserDto>> GetAllUserAsync(string token);
    Task<GetByIdAppUserDto> GetUserAsync(int id, string token);
    Task<bool> UpdateAppUserAsync(UpdateAppUserDto updateAppUserDto, string token);
    Task<bool> DeleteAppUserAsync(int id, string token);
}
