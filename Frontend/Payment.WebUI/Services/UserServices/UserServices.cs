using Newtonsoft.Json;
using Payment.WebUI.DTOs.AppUserDtos;
using System.Net.Http.Headers;
using System.Text;

namespace Payment.WebUI.Services.UserServices;

public class UserServices : IUserService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public UserServices(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<ResultAppUserDto>> GetAllUserAsync(string token)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var responseMessage = await client.GetAsync("https://localhost:7066/api/Users");

        if (responseMessage.IsSuccessStatusCode)
        {
            return await responseMessage.Content.ReadFromJsonAsync<List<ResultAppUserDto>>();
        }
        return new List<ResultAppUserDto>();
    }

    public async Task<GetByIdAppUserDto> GetUserAsync(int id, string token)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var responseMessage = await client.GetAsync($"https://localhost:7066/api/Users/{id}");

        if (responseMessage.IsSuccessStatusCode)
        {
            return await responseMessage.Content.ReadFromJsonAsync<GetByIdAppUserDto>();
        }
        return null;
    }

    public async Task<bool> UpdateAppUserAsync(UpdateAppUserDto updateAppUserDto, string token)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var jsonData = JsonConvert.SerializeObject(updateAppUserDto);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        var responseMessage = await client.PutAsync("https://localhost:7066/api/Users", content);
        return responseMessage.IsSuccessStatusCode;
    }
    public async Task<bool> DeleteAppUserAsync(int id, string token)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var responseMessage = await client.DeleteAsync($"https://localhost:7066/api/Users/{id}");
        return responseMessage.IsSuccessStatusCode;
    }

}
