using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.WebUI.DTOs.AddressDtos;
using Payment.WebUI.Services.AddressServices;
using Payment.WebUI.Services.TokenServices;

namespace Payment.WebUI.Areas.Admin.Controllers;

[Authorize(Roles = "Admin")]
[Area("Admin")]
[Route("Admin/Address")]
public class AddressController : Controller
{
    private readonly IAddressService _addressService;
    private readonly ITokenService _tokenService;

    public AddressController(IAddressService addressService, ITokenService tokenService)
    {
        _addressService = addressService;
        _tokenService = tokenService;
    }

    [HttpGet]
    [Route("Index")]
    public async Task<IActionResult> Index()
    {
        var token = _tokenService.GetTokenFromCookies(HttpContext);
        if (!_tokenService.IsTokenValid(token))
        {
            return RedirectToAction("Login", "Auth");
        }
        var values = await _addressService.GetAllAddress(token);
        return View(values);
    }

    [HttpGet]
    [Route("Create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> Create(CreateAddressDto createAddressDto)
    {
        var token = _tokenService.GetTokenFromCookies(HttpContext);
        if (!_tokenService.IsTokenValid(token))
        {
            return RedirectToAction("Login", "Auth");
        }
        var result = await _addressService.CreateAddressAsync(createAddressDto, token);
        if (result)
        {
            return RedirectToAction("Index");
        }
        return View(createAddressDto);
    }

    [HttpGet]
    [Route("Update")]
    public async Task<IActionResult> Update(int id)
    {
        var token = _tokenService.GetTokenFromCookies(HttpContext);
        if (!_tokenService.IsTokenValid(token))
        {
            return RedirectToAction("Login", "Auth");
        }
        var address = await _addressService.GetAddressByIdAsync(id, token);
        return View(address);
    }

    [HttpPost]
    [Route("Update")]
    public async Task<IActionResult> Update(UpdateAddressDto updateAddressDto)
    {
        var token = _tokenService.GetTokenFromCookies(HttpContext);
        if (!_tokenService.IsTokenValid(token))
        {
            return RedirectToAction("Login", "Auth");
        }
        var result = await _addressService.UpdateAddressAsync(updateAddressDto, token);
        if (result)
        {
            return RedirectToAction("Index");
        }
        return View(updateAddressDto);
    }

    [Route("Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var token = _tokenService.GetTokenFromCookies(HttpContext);
        if (!_tokenService.IsTokenValid(token))
        {
            return RedirectToAction("Login", "Auth");
        }
        var result = await _addressService.DeleteAddressAsync(id, token);
        if (result)
        {
            return RedirectToAction("Index");
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Route("GetJsonData")]
    public async Task<IActionResult> GetJsonData()
    {
        var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "json", "il-ilce.json");
        if (!System.IO.File.Exists(jsonFilePath))
        {
            return NotFound();
        }
        var jsonData =await System.IO.File.ReadAllTextAsync(jsonFilePath);
        return Content(jsonData, "application/json");
    }
}
