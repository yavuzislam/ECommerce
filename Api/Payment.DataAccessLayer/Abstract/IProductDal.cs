using Payment.DtoLayer.Dtos.ProductDtos;
using Payment.EntityLayer.Concrete;

namespace Payment.DataAccessLayer.Abstract
{
    public interface IProductDal: IGenericDal<Product>
    {
        Task<List<ResultProductByUserEmailDto>> GetAllByUserEmail();    
        Task<List<ResultProductByUserEmailWithCategoryNameDto>> GetAllByUserEmailWithCategoryName();    
    }
}
