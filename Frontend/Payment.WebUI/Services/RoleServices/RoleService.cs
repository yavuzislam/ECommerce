using Newtonsoft.Json;
using Payment.WebUI.DTOs.AppRoleDto;
using System.Net.Http.Headers;
using System.Text;

namespace Payment.WebUI.Services.RoleServices;

public class RoleService : IRoleService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public RoleService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<(bool,string)> CreateRoleAsync(CreateAppRoleDto createAppRoleDto, string token)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var jsonData = JsonConvert.SerializeObject(createAppRoleDto);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        var responseMessage = await client.PostAsync("https://localhost:7066/api/AppRoles", content);
        var responseContent = await responseMessage.Content.ReadAsStringAsync();
        return (responseMessage.IsSuccessStatusCode,responseContent);
    }

    public async Task<bool> DeleteRoleAsync(int id, string token)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var responseMessage = await client.DeleteAsync($"https://localhost:7066/api/AppRoles/{id}");
        return responseMessage.IsSuccessStatusCode;
    }

    public async Task<UpdateAppRoleDto> GetRoleByIdAsync(int id, string token)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var responseMessage = await client.GetAsync($"https://localhost:7066/api/AppRoles/{id}");
        if (responseMessage.IsSuccessStatusCode)
        {
            return await responseMessage.Content.ReadFromJsonAsync<UpdateAppRoleDto>();
        }
        return null;
    }

    public async Task<List<ResultAppRoleDto>> GetRolesAsync(string token)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var responseMessage = await client.GetAsync("https://localhost:7066/api/AppRoles");
        if (responseMessage.IsSuccessStatusCode)
        {
            return await responseMessage.Content.ReadFromJsonAsync<List<ResultAppRoleDto>>();
        }
        return new List<ResultAppRoleDto>();
    }

    public async Task<(bool,string)> UpdateRoleAsync(UpdateAppRoleDto updateAppRoleDto, string token)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var jsonData = JsonConvert.SerializeObject(updateAppRoleDto);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        var responseMessage = await client.PutAsync("https://localhost:7066/api/AppRoles", content);
        var responseContent = await responseMessage.Content.ReadAsStringAsync();
        return (responseMessage.IsSuccessStatusCode, responseContent);
    }
}
