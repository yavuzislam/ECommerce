using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Payment.DataAccessLayer.Abstract;
using Payment.DataAccessLayer.Concrete;
using Payment.EntityLayer.Concrete;
using System.Security.Claims;

namespace Payment.DataAccessLayer.Repositories;

public class GenericRepository<T> : IGenericDal<T> where T : BaseEntity
{
    private readonly Context _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GenericRepository(Context context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task DeleteAsync(T t)
    {
        t.IsActive = !t.IsActive;
        t.UpdateTime = DateTime.UtcNow;
        t.UpdateUserId = GetCurrentUserId();

        _context.Set<T>().Update(t);
        await _context.SaveChangesAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        //return await _context.Set<T>()
        //    .AsNoTracking()
        //    .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        return await _context.Set<T>()
            .Include(x => x.CreateUser)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<T>> GetListAsync()
    {
        return await _context.Set<T>()
            .Include(x=>x.CreateUser)
            .AsNoTracking()
            .Where(x => x.IsActive)
            .ToListAsync();
    }

    public async Task InsertAsync(T t)
    {
        int currentUserId = GetCurrentUserId();

        t.IsActive = true;
        t.CreateTime = DateTime.UtcNow;
        t.UpdateTime = DateTime.UtcNow;
        t.CreateUserId = currentUserId;
        t.UpdateUserId = currentUserId;

        await _context.Set<T>().AddAsync(t);
        await _context.SaveChangesAsync();
    }

    private int GetCurrentUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return userId != null ? int.Parse(userId) : 0;
    }

    public async Task UpdateAsync(T t)
    {
        var existingEntity = await _context.Set<T>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == t.Id);

        if (existingEntity != null)
        {
            t.CreateTime = existingEntity.CreateTime;
            t.CreateUserId = existingEntity.CreateUserId;
        }

        t.IsActive = existingEntity.IsActive;
        t.UpdateTime = DateTime.UtcNow;
        t.UpdateUserId = GetCurrentUserId();

        _context.Set<T>().Update(t);
        await _context.SaveChangesAsync();
    }
}
