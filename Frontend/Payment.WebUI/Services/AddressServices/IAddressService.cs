using Payment.WebUI.DTOs.AddressDtos;

namespace Payment.WebUI.Services.AddressServices;

public interface IAddressService
{
    Task<List<ResultAddressDto>> GetAllAddress(string token);
    Task<bool> CreateAddressAsync(CreateAddressDto createAddressDto, string token);
    Task<UpdateAddressDto> GetAddressByIdAsync(int id, string token);
    Task<bool> UpdateAddressAsync(UpdateAddressDto updateAddressDto, string token);
    Task<bool> DeleteAddressAsync(int id, string token);
}
