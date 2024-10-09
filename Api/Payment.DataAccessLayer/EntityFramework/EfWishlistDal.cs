using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Payment.DataAccessLayer.Abstract;
using Payment.DataAccessLayer.Concrete;
using Payment.DataAccessLayer.Repositories;
using Payment.DtoLayer.Dtos.WishlistDtos;
using Payment.EntityLayer.Concrete;

namespace Payment.DataAccessLayer.EntityFramework;

public class EfWishlistDal : GenericRepository<Wishlist>, IWishlistDal
{
    private readonly Context _context;
    public EfWishlistDal(Context context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
    {
        _context = context;
    }

    public async Task<List<Wishlist>> GetWishlistByUserId(int userId)
    {
        var wishlists = await _context.Wishlists
            .Include(x => x.Product)
            .Include(x => x.AppUser)
            .Where(x => x.AppUserId==userId)
            .ToListAsync();
        return wishlists;
    }
}
