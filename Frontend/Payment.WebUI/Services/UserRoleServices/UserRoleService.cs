using Payment.WebUI.Services.TokenServices;
using System.Net.Http.Headers;

namespace Payment.WebUI.Services.UserRoleServices;

public class UserRoleService : IUserRoleService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ITokenService _tokenService;

    public UserRoleService(IHttpClientFactory httpClientFactory, ITokenService tokenService)
    {
        _httpClientFactory = httpClientFactory;
        _tokenService = tokenService;
    }

    public async Task<(bool, string)> AssignRoleToUserAsync(int userId, string roleName, string token)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var responseMessage = await client.PostAsync($"https://localhost:7066/api/UserRoles/AssignRoleToUser?userId={userId}&roleName={roleName}", null);
        var responseContent = await responseMessage.Content.ReadAsStringAsync();
        return (responseMessage.IsSuccessStatusCode, responseContent);
    }

    public async Task<List<string>> GetUserRolesAsync(int userId, string token)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var responseMessage = await client.GetAsync($"https://localhost:7066/api/UserRoles?userId={userId}");
        if (responseMessage.IsSuccessStatusCode)
        {
            return await responseMessage.Content.ReadFromJsonAsync<List<string>>();
        }
        return new List<string>();
    }

    public async Task<(bool, string)> RemoveRoleFromUserAsync(int userId, string roleName, string token)
    {
        //https://localhost:7066/api/UserRoles/RemoveRoleFromUser?userId=2&roleName=Admin
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var responseMessage = await client.PostAsync($"https://localhost:7066/api/UserRoles/RemoveRoleFromUser?userId={userId}&roleName={roleName}",null);
        var responseContent = await responseMessage.Content.ReadAsStringAsync();
        return (responseMessage.IsSuccessStatusCode, responseContent);
    }
}
