using Microsoft.AspNetCore.Mvc;
using Payment.BusinessLayer.Abstract;
using Payment.DtoLayer.Dtos.CategoryDtos;
using Payment.EntityLayer.Concrete;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Payment.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public CategoriesController(ICategoryService categoryService, IMapper mapper,IFileService fileService)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _fileService = fileService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            //var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "images");
            var uploadsFolder = @"C:\Users\islam\AcunMedya\Payment\Frontend\Payment.WebUI\wwwroot\images";
            createCategoryDto.ImagePath = await _fileService.UploadFileAsync(createCategoryDto.ImageFile, uploadsFolder);

            var category = _mapper.Map<Category>(createCategoryDto);
            await _categoryService.InsertAsync(category);

            return Ok("Kategori oluşturuldu.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            var existingCategory = await _categoryService.GetByIdAsync(updateCategoryDto.Id);
            if (existingCategory == null)
            {
                return NotFound("Kategori bulunamadı.");

            }

            if (updateCategoryDto.ImageFile != null) 
            {
                var uploadsFolder = @"C:\Users\islam\AcunMedya\Payment\Frontend\Payment.WebUI\wwwroot\images";

                updateCategoryDto.ImagePath = await _fileService.UploadFileAsync(updateCategoryDto.ImageFile, uploadsFolder);

                var oldFilePath = Path.Combine(uploadsFolder, Path.GetFileName(existingCategory.ImagePath));

                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            else
            {
                updateCategoryDto.ImagePath = existingCategory.ImagePath;
            }

            var category = _mapper.Map<Category>(updateCategoryDto);
            await _categoryService.UpdateAsync(category);

            return Ok("Kategori Güncellendi");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound("Kategori bulunamadı.");
            }

            await _categoryService.DeleteAsync(category);

            return Ok("Kategori silindi.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound("Kategori bulunamadı.");

            var categoryDto = _mapper.Map<GetByIdCategoryDto>(category);
            return Ok(categoryDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var values = await _categoryService.GetListAsync();
            var categories = _mapper.Map<List<ResultCategoryDto>>(values);
            return Ok(categories);
        }

        [HttpGet("GetAllByUserEmail")]
        public async Task<IActionResult> GetAllByUserEmail()
        {
            var values = await _categoryService.TGetAllByUserEmail();
            var categories = _mapper.Map<List<ResultCategoryByUserEmailDto>>(values);
            return Ok(categories);
        }
    }
}
