using Payment.BusinessLayer.Abstract;
using Payment.DataAccessLayer.Abstract;
using Payment.EntityLayer.Concrete;
using Serilog;

namespace Payment.BusinessLayer.Concrete;

public class WishlistManager : IWishlistService
{
    private readonly IWishlistDal _wishlistDal;
    private readonly ILogger _logger;

    public WishlistManager(IWishlistDal wishlistDal)
    {
        _wishlistDal = wishlistDal;
        _logger = Log.ForContext<WishlistManager>();
    }

    public async Task DeleteAsync(Wishlist t)
    {
        if (t == null)
        {
            _logger.Warning("Wishlist nesnesi null olamaz.");
            throw new ArgumentNullException(nameof(t), "Wishlist nesnesi null olamaz.");
        }
        await _wishlistDal.DeleteAsync(t);
        _logger.Information("Wishlist başarıyla silindi. Wishlist ID: {WishlistId}", t.Id);
    }

    public async Task<Wishlist> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            _logger.Warning("Geçersiz wishlist ID sağlandı. ID: {WishlistId}", id);
            throw new ArgumentOutOfRangeException(nameof(id), "Geçersiz wishlist ID.");
        }
        var wishlist = await _wishlistDal.GetByIdAsync(id);
        if (wishlist == null)
        {
            _logger.Warning("Wishlist bulunamadı. ID: {WishlistId}", id);
            throw new NullReferenceException("Wishlist bulunamadı.");
        }
        _logger.Information("Wishlist başarıyla getirildi. Wishlist ID: {WishlistId}", id);
        return wishlist;
    }

    public async Task<List<Wishlist>> GetListAsync()
    {
        var wishlists = await _wishlistDal.GetListAsync();
        _logger.Information("Wishlistler başarıyla getirildi. Toplam wishlist sayısı: {WishlistCount}", wishlists.Count);
        return wishlists;
    }

    public async Task InsertAsync(Wishlist t)
    {
        if (t == null)
        {
            _logger.Warning("Wishlist nesnesi null olamaz.");
            throw new ArgumentNullException(nameof(t), "Wishlist nesnesi null olamaz.");
        }
        await _wishlistDal.InsertAsync(t);
    }

    public async Task<List<Wishlist>> TGetWishlistByUserId(int userId)
    {
        var wishlists = await _wishlistDal.GetWishlistByUserId(userId);
        _logger.Information("Kullanıcıya ait wishlistler başarıyla getirildi. Kullanıcı ID: {UserId}", userId);
        return wishlists;
    }

    public async Task UpdateAsync(Wishlist t)
    {
        if (t == null)
        {
            _logger.Warning("Wishlist nesnesi null olamaz.");
            throw new ArgumentNullException(nameof(t), "Wishlist nesnesi null olamaz.");
        }
        await _wishlistDal.UpdateAsync(t);
        _logger.Information("Wishlist başarıyla güncellendi. Wishlist ID: {WishlistId}", t.Id);
    }
}
