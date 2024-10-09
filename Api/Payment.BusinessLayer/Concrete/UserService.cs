using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Payment.BusinessLayer.Abstract;
using Payment.DtoLayer.Dtos.AppUserDtos;
using Payment.EntityLayer.Concrete;
using Serilog;

namespace Payment.BusinessLayer.Concrete;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public UserService(UserManager<AppUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
        _logger = Log.ForContext<UserService>();
    }

    public async Task<List<ResultAppUserDto>> GetAllUserAsync()
    {
        try
        {
            var users = await _userManager.Users.ToListAsync();
            var mappedUsers = _mapper.Map<List<ResultAppUserDto>>(users);
            _logger.Information("Tüm kullanıcılar başarıyla getirildi.");
            return mappedUsers;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Kullanıcılar getirilirken bir hata oluştu.");
            throw;
        }
    }

    public async Task<GetByIdAppUserDto> GetUserAsync(int id)
    {
        if (id <= 0)
        {
            _logger.Warning("Geçersiz kullanıcı ID: {UserId}", id);
            throw new ArgumentException("Geçersiz kullanıcı ID.", nameof(id));
        }

        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
        {
            _logger.Warning("Kullanıcı bulunamadı. Kullanıcı ID: {UserId}", id);
            throw new KeyNotFoundException("Kullanıcı bulunamadı.");
        }

        _logger.Information("Kullanıcı başarıyla getirildi. Kullanıcı ID: {UserId}", id);
        return _mapper.Map<GetByIdAppUserDto>(user);
    }

    public async Task UpdateAppUserAsync(UpdateAppUserDto updateAppUserDto)
    {
        if (updateAppUserDto == null || updateAppUserDto.Id <= 0)
        {
            _logger.Warning("Geçersiz kullanıcı güncelleme bilgileri sağlandı.");
            throw new ArgumentException("Geçersiz kullanıcı bilgileri.", nameof(updateAppUserDto));
        }

        var user = await _userManager.FindByIdAsync(updateAppUserDto.Id.ToString());
        if (user == null)
        {
            _logger.Warning("Güncellenmek istenen kullanıcı bulunamadı. Kullanıcı ID: {UserId}", updateAppUserDto.Id);
            throw new KeyNotFoundException("Kullanıcı bulunamadı.");
        }

        _mapper.Map(updateAppUserDto, user);
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            _logger.Information("Kullanıcı başarıyla güncellendi. Kullanıcı ID: {UserId}", updateAppUserDto.Id);
        }
        else
        {
            _logger.Error("Kullanıcı güncelleme sırasında bir hata oluştu. Kullanıcı ID: {UserId}", updateAppUserDto.Id);
            throw new Exception("Kullanıcı güncellenemedi.");
        }
    }

    public async Task DeleteAppUserAsync(int id)
    {
        if (id <= 0)
        {
            _logger.Warning("Geçersiz kullanıcı ID: {UserId}", id);
            throw new ArgumentException("Geçersiz kullanıcı ID.", nameof(id));
        }

        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            _logger.Warning("Silinmek istenen kullanıcı bulunamadı. Kullanıcı ID: {UserId}", id);
            throw new KeyNotFoundException("Kullanıcı bulunamadı.");
        }

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            _logger.Information("Kullanıcı başarıyla silindi. Kullanıcı ID: {UserId}", id);
        }
        else
        {
            _logger.Error("Kullanıcı silme sırasında bir hata oluştu. Kullanıcı ID: {UserId}", id);
            throw new Exception("Kullanıcı silinemedi.");
        }
    }
}
