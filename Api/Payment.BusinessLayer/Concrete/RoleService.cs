using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Payment.BusinessLayer.Abstract;
using Payment.DtoLayer.Dtos.AppRoleDto;
using Payment.DtoLayer.Dtos.AppRoleDtos;
using Payment.EntityLayer.Concrete;
using Serilog;

namespace Payment.BusinessLayer.Concrete;

public class RoleService : IRoleService
{
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager; // Kullanıcı yönetimi için ekledik
    private readonly ILogger _logger;

    public RoleService(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager; // UserManager dependency eklendi
        _logger = Log.ForContext<RoleService>();
    }

    public async Task CreateRoleAsync(CreateAppRoleDto createAppRoleDto)
    {
        if (createAppRoleDto == null || string.IsNullOrWhiteSpace(createAppRoleDto.Name))
        {
            _logger.Warning("Geçersiz rol nesnesi sağlandı.");
            throw new ArgumentNullException(nameof(createAppRoleDto), "Rol bilgileri eksik.");
        }

        // Aynı isimde bir rol varsa oluşturulmasın
        var existingRole = await _roleManager.FindByNameAsync(createAppRoleDto.Name);
        if (existingRole != null)
        {
            _logger.Warning("Aynı isimde bir rol zaten mevcut: {RoleName}", createAppRoleDto.Name);
            throw new InvalidOperationException($"'{createAppRoleDto.Name}' isimli rol zaten mevcut.");
        }

        var result = await _roleManager.CreateAsync(new AppRole { Name = createAppRoleDto.Name });
        if (result.Succeeded)
        {
            _logger.Information("Rol başarıyla eklendi. Rol Adı: {RoleName}", createAppRoleDto.Name);
        }
        else
        {
            _logger.Error("Rol ekleme sırasında bir hata oluştu. Rol Adı: {RoleName}", createAppRoleDto.Name);
            throw new Exception("Rol eklenemedi.");
        }
    }

    public async Task DeleteRoleAsync(int id)
    {
        if (id <= 0)
        {
            _logger.Warning("Geçersiz role ID: {AppRoleID}", id);
            throw new ArgumentException("Geçersiz role ID.", nameof(id));
        }

        var role = await _roleManager.FindByIdAsync(id.ToString());
        if (role == null)
        {
            _logger.Warning("Silinmek istenen rol bulunamadı. ID: {AppRoleID}", id);
            throw new KeyNotFoundException("Rol bulunamadı.");
        }

        // Role atanmış kullanıcı olup olmadığını kontrol et
        var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
        if (usersInRole.Any())
        {
            _logger.Warning("Silinmek istenen role atanmış kullanıcılar mevcut. Rol ID: {AppRoleID}", id);
            throw new InvalidOperationException("Bu role atanmış kullanıcılar var. Rol silinemez.");
        }

        var result = await _roleManager.DeleteAsync(role);
        if (result.Succeeded)
        {
            _logger.Information("Rol başarıyla silindi. Rol ID: {AppRoleID}", id);
        }
        else
        {
            _logger.Error("Rol silme sırasında bir hata oluştu. Rol ID: {AppRoleID}", id);
            throw new Exception("Rol silinemedi.");
        }
    }

    public async Task<GetByIdAppRoleDto> GetRoleByIdAsync(int id)
    {
        if (id <= 0)
        {
            _logger.Warning("Geçersiz role ID: {AppRoleID}", id);
            throw new ArgumentException("Geçersiz role ID.", nameof(id));
        }

        var role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == id);
        if (role == null)
        {
            _logger.Warning("İstenen rol bulunamadı. Rol ID: {AppRoleID}", id);
            throw new KeyNotFoundException("Rol bulunamadı.");
        }

        _logger.Information("Rol başarıyla getirildi. Rol ID: {AppRoleID}", id);
        return new GetByIdAppRoleDto
        {
            Id = role.Id,
            Name = role.Name
        };
    }

    public async Task<List<ResultAppRoleDto>> GetRolesAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        _logger.Information("Roller başarıyla getirildi.");
        return roles.Select(x => new ResultAppRoleDto
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();
    }

    public async Task UpdateRoleAsync(UpdateAppRoleDto updateAppRoleDto)
    {
        if (updateAppRoleDto == null || updateAppRoleDto.Id <= 0 || string.IsNullOrWhiteSpace(updateAppRoleDto.Name))
        {
            _logger.Warning("Geçersiz rol güncelleme bilgileri sağlandı.");
            throw new ArgumentNullException(nameof(updateAppRoleDto), "Rol bilgileri eksik.");
        }

        var role = await _roleManager.FindByIdAsync(updateAppRoleDto.Id.ToString());
        if (role == null)
        {
            _logger.Warning("Güncellenmek istenen rol bulunamadı. ID: {AppRoleID}", updateAppRoleDto.Id);
            throw new KeyNotFoundException("Rol bulunamadı.");
        }

        // Aynı isimde başka bir rol varsa güncellenmesin
        var existingRole = await _roleManager.FindByNameAsync(updateAppRoleDto.Name);
        if (existingRole != null && existingRole.Id != updateAppRoleDto.Id)
        {
            _logger.Warning("Aynı isimde başka bir rol mevcut. Güncellenmek istenen rol ID: {AppRoleID}", updateAppRoleDto.Id);
            throw new InvalidOperationException($"'{updateAppRoleDto.Name}' isimli rol zaten mevcut.");
        }

        role.Name = updateAppRoleDto.Name;
        var result = await _roleManager.UpdateAsync(role);
        if (result.Succeeded)
        {
            _logger.Information("Rol başarıyla güncellendi. Rol ID: {AppRoleID}", updateAppRoleDto.Id);
        }
        else
        {
            _logger.Error("Rol güncelleme sırasında bir hata oluştu. Rol ID: {AppRoleID}", updateAppRoleDto.Id);
            throw new Exception("Rol güncellenemedi.");
        }
    }
}
