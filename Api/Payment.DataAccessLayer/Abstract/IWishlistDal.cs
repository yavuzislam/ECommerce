using Payment.DtoLayer.Dtos.WishlistDtos;
using Payment.EntityLayer.Concrete;

namespace Payment.DataAccessLayer.Abstract;

public interface IWishlistDal : IGenericDal<Wishlist>
{
    Task<List<Wishlist>> GetWishlistByUserId(int userId);
}
