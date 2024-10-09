using Payment.WebUI.DTOs.CategoryDtos;

namespace Payment.WebUI.Services.CategoryServices;

public interface ICategoryService
{
    Task<List<ResultCategoryByUserEmailDto>> GetAllCategoriesByUserEmailAsync(string token);
    Task<bool> CreateCategoryAsync(CreateCategoryDto createCategoryDto, string token);
    Task<UpdateCategoryDto> GetCategoryByIdAsync(int id, string token);
    Task<bool> UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto, string token);
    Task<bool> DeleteCategoryAsync(int id, string token);
}

