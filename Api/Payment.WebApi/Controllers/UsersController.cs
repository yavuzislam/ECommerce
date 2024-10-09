using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.BusinessLayer.Abstract;
using Payment.DtoLayer.Dtos.AppUserDtos;


namespace Payment.WebApi.Controllers;

[Authorize(Roles ="Admin")]
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpGet]
    public async Task<List<ResultAppUserDto>> GetAllUsers()
    {
        var users = await _userService.GetAllUserAsync();
        return users;
    }

    [HttpGet("{id}")]
    public async Task<GetByIdAppUserDto> GetUser(int id)
    {
        var user = await _userService.GetUserAsync(id);
        return user;
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser(UpdateAppUserDto updateAppUserDto)
    {
        await _userService.UpdateAppUserAsync(updateAppUserDto);
        return Ok("Kullanıcı başarıyla güncellendi.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _userService.DeleteAppUserAsync(id);
        return Ok("Kullanıcı başarıyla silindi.");
    }

}
