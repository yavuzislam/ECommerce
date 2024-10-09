using Payment.WebUI.DTOs.AppRoleDto;

namespace Payment.WebUI.Services.RoleServices;

public interface IRoleService
{
    Task<List<ResultAppRoleDto>> GetRolesAsync(string token);
    Task<UpdateAppRoleDto> GetRoleByIdAsync(int id, string token);
    Task<(bool,string)> CreateRoleAsync(CreateAppRoleDto createAppRoleDto, string token);
    Task<(bool, string)> UpdateRoleAsync(UpdateAppRoleDto updateAppRoleDto, string token);
    Task<bool> DeleteRoleAsync(int id, string token);
}
