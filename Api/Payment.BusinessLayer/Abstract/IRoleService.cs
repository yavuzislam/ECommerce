using Payment.DtoLayer.Dtos.AppRoleDto;
using Payment.DtoLayer.Dtos.AppRoleDtos;

namespace Payment.BusinessLayer.Abstract;

public interface IRoleService
{
    Task<List<ResultAppRoleDto>> GetRolesAsync();
    Task<GetByIdAppRoleDto> GetRoleByIdAsync(int id);
    Task CreateRoleAsync(CreateAppRoleDto createAppRoleDto);
    Task UpdateRoleAsync(UpdateAppRoleDto updateAppRoleDto);
    Task DeleteRoleAsync(int id);
}
