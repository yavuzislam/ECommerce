using Payment.WebUI.DTOs.ProductDtos;
using System.Net.Http.Headers;

namespace Payment.WebUI.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<List<ResultProductByUserEmailWithCategoryNameDto>> GetAllCategoriesByUserEmailWithCategoryNameAsync(string token)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await client.GetAsync("https://localhost:7066/api/Products/GetAllByUserEmailWithCategoryName");

            if (responseMessage.IsSuccessStatusCode)
            {
                return await responseMessage.Content.ReadFromJsonAsync<List<ResultProductByUserEmailWithCategoryNameDto>>();
            }
            return new List<ResultProductByUserEmailWithCategoryNameDto>();
        }

        public async Task<UpdateProductDto> GetProductByIdAsync(int id, string token)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await client.GetAsync($"https://localhost:7066/api/Products/{id}");

            if (responseMessage.IsSuccessStatusCode)
            {
                return await responseMessage.Content.ReadFromJsonAsync<UpdateProductDto>();
            }
            return null;
        }

        public async Task<bool> CreateProductAsync(CreateProductDto createProductDto, string token)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new MultipartFormDataContent();

            content.Add(new StringContent(createProductDto.Name), "Name");
            content.Add(new StringContent(createProductDto.Description ?? ""), "Description");
            content.Add(new StringContent(createProductDto.Price.ToString()), "Price");
            content.Add(new StringContent(createProductDto.DiscountRate.ToString() ?? ""), "DiscountRate");
            content.Add(new StringContent(createProductDto.Stock.ToString()), "Stock");
            content.Add(new StringContent(createProductDto.CategoryId.ToString()), "CategoryId");

            if (createProductDto.ImageFile != null && createProductDto.ImageFile.Length > 0)
            {
                var fileStreamContent = new StreamContent(createProductDto.ImageFile.OpenReadStream());
                fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(createProductDto.ImageFile.ContentType);
                content.Add(fileStreamContent, "ImageFile", createProductDto.ImageFile.FileName);
            }

            var responseMessage = await client.PostAsync("https://localhost:7066/api/Products/", content);
            return responseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProductAsync(int id, string token)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await client.DeleteAsync($"https://localhost:7066/api/Products/{id}");
            return responseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProductAsync(UpdateProductDto updateProductDto, string token)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new MultipartFormDataContent();

            content.Add(new StringContent(updateProductDto.Id.ToString()), "Id");
            content.Add(new StringContent(updateProductDto.Name), "Name");
            content.Add(new StringContent(updateProductDto.Description ?? ""), "Description");
            content.Add(new StringContent(updateProductDto.Price.ToString()), "Price");
            content.Add(new StringContent(updateProductDto.DiscountRate.ToString() ?? ""), "DiscountRate");
            content.Add(new StringContent(updateProductDto.Stock.ToString()), "Stock");
            content.Add(new StringContent(updateProductDto.CategoryId.ToString()), "CategoryId");

            if (updateProductDto.ImageFile != null && updateProductDto.ImageFile.Length > 0)
            {
                var fileStreamContent = new StreamContent(updateProductDto.ImageFile.OpenReadStream());
                fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(updateProductDto.ImageFile.ContentType);
                content.Add(fileStreamContent, "ImageFile", updateProductDto.ImageFile.FileName);
            }

            var responseMessage = await client.PutAsync($"https://localhost:7066/api/Products/", content);
            return responseMessage.IsSuccessStatusCode;
        }
    }
}
