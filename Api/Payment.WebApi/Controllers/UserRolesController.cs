using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.BusinessLayer.Abstract;


namespace Payment.WebApi.Controllers;

[Authorize(Roles ="Admin")]
[Route("api/[controller]")]
[ApiController]
public class UserRolesController : ControllerBase
{
    private readonly IUserRoleService _userRoleService;

    public UserRolesController(IUserRoleService userRoleService)
    {
        _userRoleService = userRoleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserRoles(int userId)
    {
        var roles = await _userRoleService.GetUserRolesAsync(userId);
        return Ok(roles);
    }

    [HttpPost("AssignRoleToUser")]
    public async Task<IActionResult> AssignRoleToUser(int userId, string roleName)
    {
        await _userRoleService.AssignRoleToUserAsync(userId, roleName);
        return Ok("Rol başarıyla eklendi.");
    }

    [HttpPost("RemoveRoleFromUser")]
    public async Task<IActionResult> RemoveRoleFromUser(int userId, string roleName)
    {
        await _userRoleService.RemoveRoleFromUserAsync(userId, roleName);
        return Ok("Rol başarıyla silindi.");
    }

}
