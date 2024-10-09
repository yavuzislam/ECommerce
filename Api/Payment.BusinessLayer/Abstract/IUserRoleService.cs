namespace Payment.BusinessLayer.Abstract;

public interface IUserRoleService
{
    Task AssignRoleToUserAsync(int userId, string roleName);
    Task RemoveRoleFromUserAsync(int userId, string roleName);
    Task<List<string>> GetUserRolesAsync(int userId);
}
