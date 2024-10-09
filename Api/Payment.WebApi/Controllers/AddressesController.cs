using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.BusinessLayer.Abstract;
using Payment.DtoLayer.Dtos.AddressDtos;
using Payment.EntityLayer.Concrete;


namespace Payment.WebApi.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class AddressesController : ControllerBase
{
    private readonly IAddressService _addressService;
    private readonly IMapper _mapper;

    public AddressesController(IAddressService addressService, IMapper mapper)
    {
        _addressService = addressService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAddresses()
    {
        var values = await _addressService.GetListAsync();
        var addresses = _mapper.Map<List<ResultAddressDto>>(values);
        return Ok(addresses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAddressById(int id)
    {
        var value = await _addressService.GetByIdAsync(id);
        if (value == null)
        {
            return NotFound("Adres bulunamadı.");
        }
        var address = _mapper.Map<GetByIdAddressDto>(value);
        return Ok(address);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAddress(CreateAddressDto createAddressDto)
    {
        var address = _mapper.Map<Address>(createAddressDto);
        await _addressService.InsertAsync(address);
        return Ok("Adres oluşturuldu.");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAddress(UpdateAddressDto updateAddressDto)
    {
        var existingAddress = await _addressService.GetByIdAsync(updateAddressDto.Id);
        if (existingAddress == null)
        {
            return NotFound("Adres bulunamadı.");
        }

        var address = _mapper.Map<Address>(updateAddressDto);
        await _addressService.UpdateAsync(address);
        return Ok("Adres güncellendi.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAddress(int id)
    {
        var existingAddress = await _addressService.GetByIdAsync(id);
        if (existingAddress == null)
        {
            return NotFound("Adres bulunamadı.");
        }

        await _addressService.DeleteAsync(existingAddress);
        return Ok("Adres silindi.");
    }
}
