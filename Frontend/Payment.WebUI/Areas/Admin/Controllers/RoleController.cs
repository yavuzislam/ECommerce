using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.WebUI.DTOs.AppRoleDto;
using Payment.WebUI.Services.RoleServices;
using Payment.WebUI.Services.TokenServices;

namespace Payment.WebUI.Areas.Admin.Controllers;

[Authorize(Roles = "Admin")]
[Area("Admin")]
[Route("Admin/Role")]
public class RoleController : Controller
{
    private readonly IRoleService _roleService;
    private readonly ITokenService _tokenService;

    public RoleController(IRoleService roleService, ITokenService tokenService)
    {
        _roleService = roleService;
        _tokenService = tokenService;
    }

    [HttpGet]
    [Route("Index")]
    public async Task<IActionResult> Index()
    {
        var token = _tokenService.GetTokenFromCookies(HttpContext);
        if (!_tokenService.IsTokenValid(token))
        {
            return RedirectToAction("Login", "Auth");
        }
        var roles = await _roleService.GetRolesAsync(token);
        return View(roles);
    }

    [HttpGet]
    [Route("Create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> Create(CreateAppRoleDto createAppRoleDto)
    {
        var token = _tokenService.GetTokenFromCookies(HttpContext);
        if (!_tokenService.IsTokenValid(token))
        {
            return RedirectToAction("Login", "Auth");
        }
        if (!ModelState.IsValid)
        {
            return View(createAppRoleDto);
        }
        var (isSuccess, errorMessage) = await _roleService.CreateRoleAsync(createAppRoleDto, token);
        if (isSuccess)
        {
            return RedirectToAction("Index");
        }
        ViewBag.ErrorMessage = errorMessage;
        return View(createAppRoleDto);
    }

    [HttpGet]
    [Route("Update")]
    public async Task<IActionResult> Update(int id)
    {
        var token = _tokenService.GetTokenFromCookies(HttpContext);
        if (!_tokenService.IsTokenValid(token))
        {
            return RedirectToAction("Login", "Auth");
        }
        var role = await _roleService.GetRoleByIdAsync(id, token);
        return View(role);
    }

    [HttpPost]
    [Route("Update")]
    public async Task<IActionResult> Update(UpdateAppRoleDto updateAppRoleDto)
    {
        var token = _tokenService.GetTokenFromCookies(HttpContext);
        if (!_tokenService.IsTokenValid(token))
        {
            return RedirectToAction("Login", "Auth");
        }
        if (!ModelState.IsValid)
        {
            return View(updateAppRoleDto);
        }
        var (isSuccess, errorMessage) = await _roleService.UpdateRoleAsync(updateAppRoleDto, token);
        if (isSuccess)
        {
            return RedirectToAction("Index");
        }
        ViewBag.ErrorMessage = errorMessage;
        return View(updateAppRoleDto);
    }
    [Route("Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var token = _tokenService.GetTokenFromCookies(HttpContext);
        if (!_tokenService.IsTokenValid(token))
        {
            return RedirectToAction("Login", "Auth");
        }
        var isSuccess = await _roleService.DeleteRoleAsync(id, token);
        if (isSuccess)
        {
            return RedirectToAction("Index");
        }
        return View();
    }
}
