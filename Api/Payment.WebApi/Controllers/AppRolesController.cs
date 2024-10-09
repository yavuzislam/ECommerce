using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.BusinessLayer.Abstract;
using Payment.DtoLayer.Dtos.AppRoleDto;


namespace Payment.WebApi.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class AppRolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public AppRolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAppRoles()
    {
        var roles = await _roleService.GetRolesAsync();
        return Ok(roles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRole(int id)
    {
        var role = await _roleService.GetRoleByIdAsync(id);
        return Ok(role);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole(CreateAppRoleDto createAppRoleDto)
     {
        await _roleService.CreateRoleAsync(createAppRoleDto);
        return Ok("Rol başarıyla eklendi.");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRole(UpdateAppRoleDto updateAppRoleDto)
    {
        await _roleService.UpdateRoleAsync(updateAppRoleDto);
        return Ok("Rol başarıyla güncellendi.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        await _roleService.DeleteRoleAsync(id);
        return Ok("Rol başarıyla silindi.");
    }
}
