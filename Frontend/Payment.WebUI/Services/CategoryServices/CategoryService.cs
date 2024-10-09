using Payment.WebUI.DTOs.CategoryDtos;
using System.Net.Http.Headers;

namespace Payment.WebUI.Services.CategoryServices;

public class CategoryService : ICategoryService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CategoryService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<List<ResultCategoryByUserEmailDto>> GetAllCategoriesByUserEmailAsync(string token)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var responseMessage = await client.GetAsync("https://localhost:7066/api/Categories/GetAllByUserEmail");

        if (responseMessage.IsSuccessStatusCode)
        {
            return await responseMessage.Content.ReadFromJsonAsync<List<ResultCategoryByUserEmailDto>>();
        }
        return new List<ResultCategoryByUserEmailDto>();
    }

    public async Task<UpdateCategoryDto> GetCategoryByIdAsync(int id, string token)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var responseMessage = await client.GetAsync($"https://localhost:7066/api/Categories/{id}");

        if (responseMessage.IsSuccessStatusCode)
        {
            return await responseMessage.Content.ReadFromJsonAsync<UpdateCategoryDto>();
        }
        return null;
    }

    public async Task<bool> CreateCategoryAsync(CreateCategoryDto createCategoryDto, string token)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var content = new MultipartFormDataContent();


        // Diğer form verilerini ekleyin
        content.Add(new StringContent(createCategoryDto.Name), "Name");
        content.Add(new StringContent(createCategoryDto.Description ?? ""), "Description");

        // Resim dosyasını ekleyin
        if (createCategoryDto.ImageFile != null && createCategoryDto.ImageFile.Length > 0)
        {
            // Dosyayı form-data'ya ekle
            var fileStreamContent = new StreamContent(createCategoryDto.ImageFile.OpenReadStream());
            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(createCategoryDto.ImageFile.ContentType);
            content.Add(fileStreamContent, "ImageFile", createCategoryDto.ImageFile.FileName);
        }



        var responseMessage = await client.PostAsync("https://localhost:7066/api/Categories/", content);
        return responseMessage.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCategoryAsync(int id, string token)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var responseMessage = await client.DeleteAsync($"https://localhost:7066/api/Categories/{id}");
        return responseMessage.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto, string token)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


        var content = new MultipartFormDataContent();

        // Diğer form verilerini ekleyin
        content.Add(new StringContent(updateCategoryDto.Id.ToString()), "Id");
        content.Add(new StringContent(updateCategoryDto.Name), "Name");
        content.Add(new StringContent(updateCategoryDto.Description ?? ""), "Description");

        // Resim dosyasını ekleyin (eğer varsa)
        if (updateCategoryDto.ImageFile != null)
        {
            var imageContent = new StreamContent(updateCategoryDto.ImageFile.OpenReadStream());
            imageContent.Headers.ContentType = new MediaTypeHeaderValue(updateCategoryDto.ImageFile.ContentType);
            content.Add(imageContent, "ImageFile", updateCategoryDto.ImageFile.FileName);
        }

        var responseMessage = await client.PutAsync($"https://localhost:7066/api/Categories/", content);
        return responseMessage.IsSuccessStatusCode;
    }
}
