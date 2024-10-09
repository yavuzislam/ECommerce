using Payment.EntityLayer.Concrete;

namespace Payment.DataAccessLayer.Abstract;

public interface IUserDal
{
    Task<List<AppUser>> GetUsersAsync();
    Task<AppUser> GetUserByIdAsync(int id);
    Task InsertUserAsync(AppUser user);
    Task UpdateUserAsync(AppUser user);
    Task DeleteUserAsync(AppUser user);
}
