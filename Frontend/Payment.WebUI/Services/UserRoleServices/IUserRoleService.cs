namespace Payment.WebUI.Services.UserRoleServices;

public interface IUserRoleService
{
    Task<(bool, string)> AssignRoleToUserAsync(int userId, string roleName, string token);
    Task<(bool, string)> RemoveRoleFromUserAsync(int userId, string roleName, string token);
    Task<List<string>> GetUserRolesAsync(int userId, string token);
}
