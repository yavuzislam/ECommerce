using Payment.EntityLayer.Concrete;

namespace Payment.BusinessLayer.Abstract;

public interface IWishlistService: IGenericService<Wishlist>
{
    Task<List<Wishlist>> TGetWishlistByUserId(int userId);
}
