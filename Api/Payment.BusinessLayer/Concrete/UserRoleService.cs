using Microsoft.AspNetCore.Identity;
using Payment.BusinessLayer.Abstract;
using Payment.EntityLayer.Concrete;
using Serilog;

namespace Payment.BusinessLayer.Concrete;

public class UserRoleService : IUserRoleService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly ILogger _logger;

    public UserRoleService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = Log.ForContext<UserRoleService>();
    }

    // Kullanıcıya rol atama
    public async Task AssignRoleToUserAsync(int userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            _logger.Warning("Kullanıcı bulunamadı. Kullanıcı ID: {UserId}", userId);
            throw new KeyNotFoundException("Kullanıcı bulunamadı.");
        }

        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            _logger.Warning("Rol bulunamadı. Rol Adı: {RoleName}", roleName);
            throw new KeyNotFoundException("Rol bulunamadı.");
        }

        var result = await _userManager.AddToRoleAsync(user, roleName);
        if (result.Succeeded)
        {
            _logger.Information("Kullanıcıya rol başarıyla atandı. Kullanıcı ID: {UserId}, Rol Adı: {RoleName}", userId, roleName);
        }
        else
        {
            _logger.Error("Kullanıcıya rol atama sırasında bir hata oluştu. Kullanıcı ID: {UserId}, Rol Adı: {RoleName}", userId, roleName);
            throw new Exception("Rol atama başarısız oldu.");
        }
    }

    // Kullanıcıdan rol kaldırma
    public async Task RemoveRoleFromUserAsync(int userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            _logger.Warning("Kullanıcı bulunamadı. Kullanıcı ID: {UserId}", userId);
            throw new KeyNotFoundException("Kullanıcı bulunamadı.");
        }

        var result = await _userManager.RemoveFromRoleAsync(user, roleName);
        if (result.Succeeded)
        {
            _logger.Information("Kullanıcıdan rol başarıyla kaldırıldı. Kullanıcı ID: {UserId}, Rol Adı: {RoleName}", userId, roleName);
        }
        else
        {
            _logger.Error("Kullanıcıdan rol kaldırma sırasında bir hata oluştu. Kullanıcı ID: {UserId}, Rol Adı: {RoleName}", userId, roleName);
            throw new Exception("Rol kaldırma başarısız oldu.");
        }
    }

    // Kullanıcının rollerini getirme
    public async Task<List<string>> GetUserRolesAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            _logger.Warning("Kullanıcı bulunamadı. Kullanıcı ID: {UserId}", userId);
            throw new KeyNotFoundException("Kullanıcı bulunamadı.");
        }

        var roles = await _userManager.GetRolesAsync(user);
        _logger.Information("Kullanıcının roller başarıyla getirildi. Kullanıcı ID: {UserId}", userId);
        return roles.ToList();
    }
}