using Payment.WebUI.DTOs.AppRoleDto;

namespace Payment.WebUI.Models;

public class UserRoleViewModel
{
    public int UserId { get; set; }
    public List<string> UserRoles { get; set; }
    public List<ResultAppRoleDto> AllRoles { get; set; }
}
