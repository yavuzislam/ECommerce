using Payment.DtoLayer.Dtos.ProductDtos;
using Payment.EntityLayer.Concrete;

namespace Payment.BusinessLayer.Abstract;

public interface IProductService: IGenericService<Product>
{
    Task<List<ResultProductByUserEmailDto>> TGetAllByUserEmail();
    Task<List<ResultProductByUserEmailWithCategoryNameDto>> TGetAllByUserEmailWithCategoryName();
}
