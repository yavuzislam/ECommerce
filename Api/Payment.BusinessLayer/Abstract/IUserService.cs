using Payment.DtoLayer.Dtos.AppUserDtos;
using Payment.EntityLayer.Concrete;

namespace Payment.BusinessLayer.Abstract;

public interface IUserService
{
    Task<List<ResultAppUserDto>> GetAllUserAsync();
    Task<GetByIdAppUserDto> GetUserAsync(int id);
    Task UpdateAppUserAsync(UpdateAppUserDto updateAppUserDto);
    Task DeleteAppUserAsync(int id);
}
