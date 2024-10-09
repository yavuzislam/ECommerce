using Payment.WebUI.DTOs.AddressDtos;
using System.Net.Http.Headers;

namespace Payment.WebUI.Services.AddressServices;

public class AddressService : IAddressService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AddressService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<bool> CreateAddressAsync(CreateAddressDto createAddressDto, string token)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var responseMessage = await client.PostAsJsonAsync("https://localhost:7066/api/Addresses", createAddressDto);
        return responseMessage.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAddressAsync(int id, string token)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var responseMessage = await client.DeleteAsync($"https://localhost:7066/api/Addresses/{id}");
        return responseMessage.IsSuccessStatusCode;
    }

    public async Task<UpdateAddressDto> GetAddressByIdAsync(int id, string token)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var responseMessage = await client.GetAsync($"https://localhost:7066/api/Addresses/{id}");

        if (responseMessage.IsSuccessStatusCode)
        {
            return await responseMessage.Content.ReadFromJsonAsync<UpdateAddressDto>();
        }

        return null;
    }

    public async Task<List<ResultAddressDto>> GetAllAddress(string token)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var responseMessage = await client.GetAsync("https://localhost:7066/api/Addresses");

        if (responseMessage.IsSuccessStatusCode)
        {
            return await responseMessage.Content.ReadFromJsonAsync<List<ResultAddressDto>>();
        }

        return new List<ResultAddressDto>();
    }

    public async Task<bool> UpdateAddressAsync(UpdateAddressDto updateAddressDto, string token)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var responseMessage = await client.PutAsJsonAsync("https://localhost:7066/api/Addresses", updateAddressDto);
        return responseMessage.IsSuccessStatusCode;
    }
}
