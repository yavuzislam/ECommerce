using Payment.DtoLayer.Dtos.CategoryDtos;
using Payment.EntityLayer.Concrete;

namespace Payment.BusinessLayer.Abstract;

public interface ICategoryService : IGenericService<Category>
{
    Task<List<ResultCategoryByUserEmailDto>> TGetAllByUserEmail();
}
