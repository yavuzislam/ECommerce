using Payment.WebUI.DTOs.ProductDtos;

namespace Payment.WebUI.Services.ProductServices;

public interface IProductService
{
    Task<List<ResultProductByUserEmailWithCategoryNameDto>> GetAllCategoriesByUserEmailWithCategoryNameAsync(string token);
    Task<bool> CreateProductAsync(CreateProductDto createProductDto, string token);
    Task<UpdateProductDto> GetProductByIdAsync(int id, string token);
    Task<bool> UpdateProductAsync(UpdateProductDto updateProductDto, string token);
    Task<bool> DeleteProductAsync(int id, string token);
}
