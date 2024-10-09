using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.WebUI.Models;
using Payment.WebUI.Services.RoleServices;
using Payment.WebUI.Services.TokenServices;
using Payment.WebUI.Services.UserRoleServices;
using Payment.WebUI.Services.UserServices;

namespace Payment.WebUI.Areas.Admin.Controllers;

[Authorize(Roles = "Admin")]
[Area("Admin")]
[Route("Admin/UserRole")]
public class UserRoleController : Controller
{
    private readonly IUserRoleService _userRoleService;
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;

    public UserRoleController(IUserRoleService userRoleService, ITokenService tokenService, IUserService userService, IRoleService roleService)
    {
        _userRoleService = userRoleService;
        _tokenService = tokenService;
        _userService = userService;
        _roleService = roleService;
    }

    [HttpGet]
    [Route("Index")]
    public async Task<IActionResult> Index(int userId)
    {
        var token = _tokenService.GetTokenFromCookies(HttpContext);
        if (!_tokenService.IsTokenValid(token))
        {
            return RedirectToAction("Login", "Auth");
        }
        var userRoles = await _userRoleService.GetUserRolesAsync(userId, token);

        var allRoles = await _roleService.GetRolesAsync(token);

        var model = new UserRoleViewModel
        {
            UserId = userId,
            UserRoles = userRoles,
            AllRoles = allRoles
        };

        return View(model);
    }

    [HttpPost]
    [Route("UpdateRoles")]
    public async Task<IActionResult> UpdateRoles(int userId, List<string> roles)
    {
        var token = _tokenService.GetTokenFromCookies(HttpContext);
        if (!_tokenService.IsTokenValid(token))
        {
            return RedirectToAction("Login", "Auth");
        }
        var userRoles = await _userRoleService.GetUserRolesAsync(userId, token);

        var rolesToAdd = roles.Except(userRoles).ToList();
        var rolesToRemove = userRoles.Except(roles).ToList();

        foreach (var role in rolesToAdd)
        {
            await _userRoleService.AssignRoleToUserAsync(userId, role, token);
        }

        foreach (var role in rolesToRemove)
        {
            await _userRoleService.RemoveRoleFromUserAsync(userId, role, token);
        }
        return RedirectToAction("Index", new { userId = userId });
    }
}
