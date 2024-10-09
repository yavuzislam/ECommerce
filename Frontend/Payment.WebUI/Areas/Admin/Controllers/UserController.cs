using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.WebUI.DTOs.AppUserDtos;
using Payment.WebUI.Services.TokenServices;
using Payment.WebUI.Services.UserServices;

namespace Payment.WebUI.Areas.Admin.Controllers;

[Authorize(Roles = "Admin")]
[Area("Admin")]
[Route("Admin/User")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public UserController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
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

        var users = await _userService.GetAllUserAsync(token);
        return View(users);
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

        var user = await _userService.GetUserAsync(id, token);
        return View(user);
    }

    [HttpPost]
    [Route("Update")]
    public async Task<IActionResult> Update(UpdateAppUserDto updateUserDto)
    {
        var token = _tokenService.GetTokenFromCookies(HttpContext);
        if (!_tokenService.IsTokenValid(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        var isSucces = await _userService.UpdateAppUserAsync(updateUserDto, token);
        if (isSucces)
        {
            return RedirectToAction("Index");
        }
        return View(updateUserDto);
    }

    [Route("Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var token = _tokenService.GetTokenFromCookies(HttpContext);
        if (!_tokenService.IsTokenValid(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        var isSucces = await _userService.DeleteAppUserAsync(id, token);
        if (isSucces)
        {
            return RedirectToAction("Index");
        }
        return View();
    }
}
