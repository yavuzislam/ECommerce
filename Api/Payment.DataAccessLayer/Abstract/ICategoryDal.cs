using Payment.DtoLayer.Dtos.CategoryDtos;
using Payment.EntityLayer.Concrete;

namespace Payment.DataAccessLayer.Abstract
{
    public interface ICategoryDal : IGenericDal<Category>
    {
        Task<List<ResultCategoryByUserEmailDto>> GetAllByUserEmail();
    }
}
