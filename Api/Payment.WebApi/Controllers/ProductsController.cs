using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.BusinessLayer.Abstract;
using Payment.DtoLayer.Dtos.ProductDtos;
using Payment.EntityLayer.Concrete;


namespace Payment.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public ProductsController(IProductService productService, IMapper mapper, IFileService fileService)
        {
            _productService = productService;
            _mapper = mapper;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var values = await _productService.GetListAsync();
            var products = _mapper.Map<List<ResultProductDto>>(values);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var value = await _productService.GetByIdAsync(id);
            if (value == null)
                return NotFound("Ürün bulunamadı.");

            var product = _mapper.Map<GetByIdProductDto>(value);
            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto)
        {
            var uploadsFolder = @"C:\Users\islam\AcunMedya\Payment\Frontend\Payment.WebUI\wwwroot\images";
            createProductDto.ImagePath = await _fileService.UploadFileAsync(createProductDto.ImageFile, uploadsFolder);

            var product = _mapper.Map<Product>(createProductDto);
            await _productService.InsertAsync(product);

            return Ok("Ürün oluşturuldu.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto)
        {
            var existingCategory = await _productService.GetByIdAsync(updateProductDto.Id);
            if (existingCategory == null)
            {
                return NotFound("Kategori bulunamadı.");

            }

            if (updateProductDto.ImageFile != null)
            {
                var uploadsFolder = @"C:\Users\islam\AcunMedya\Payment\Frontend\Payment.WebUI\wwwroot\images";

                updateProductDto.ImagePath = await _fileService.UploadFileAsync(updateProductDto.ImageFile, uploadsFolder);

                var oldFilePath = Path.Combine(uploadsFolder, Path.GetFileName(existingCategory.ImagePath));

                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            else
            {
                updateProductDto.ImagePath = existingCategory.ImagePath;
            }

            var product = _mapper.Map<Product>(updateProductDto);
            await _productService.UpdateAsync(product);

            return Ok("Ürün Güncellendi");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound("Ürün bulunamadı.");

            await _productService.DeleteAsync(product);

            return Ok("Ürün silindi.");
        }

        [HttpGet("GetAllByUserEmail")]
        public async Task<IActionResult> GetAllByUserEmail()
        {
            var values = await _productService.TGetAllByUserEmail();
            var products = _mapper.Map<List<ResultProductByUserEmailDto>>(values);
            return Ok(products);
        }

        [HttpGet("GetAllByUserEmailWithCategoryName")]
        public async Task<IActionResult> GetAllByUserEmailWithCategoryName()
        {
            var values = await _productService.TGetAllByUserEmailWithCategoryName();
            var products = _mapper.Map<List<ResultProductByUserEmailWithCategoryNameDto>>(values);
            return Ok(products);
        }
    }
}
