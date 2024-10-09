using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.WebUI.DTOs.ProductDtos;
using Payment.WebUI.Services.ProductServices;
using Payment.WebUI.Services.TokenServices;

namespace Payment.WebUI.Areas.Admin.Controllers;

[Authorize(Roles = "Admin")]
[Area("Admin")]
[Route("Admin/Product")]
public class ProductController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IProductService _ProductService;
    private readonly ITokenService _tokenService;

    public ProductController(IHttpClientFactory httpClientFactory, IProductService ProductService, ITokenService tokenService)
    {
        _httpClientFactory = httpClientFactory;
        _ProductService = ProductService;
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

        var categories = await _ProductService.GetAllCategoriesByUserEmailWithCategoryNameAsync(token);
        return View(categories);
    }

    [HttpGet]
    [Route("Create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> Create(CreateProductDto createProductDto)
    {
        var token = _tokenService.GetTokenFromCookies(HttpContext);
        if (!_tokenService.IsTokenValid(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        var isSuccess = await _ProductService.CreateProductAsync(createProductDto, token);
        if (isSuccess)
        {
            return RedirectToAction("Index");
        }
        return View(createProductDto);
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

        var Product = await _ProductService.GetProductByIdAsync(id, token);
        return View(Product);
    }

    [HttpPost]
    [Route("Update")]
    public async Task<IActionResult> Update(UpdateProductDto updateProductDto)
    {
        var token = _tokenService.GetTokenFromCookies(HttpContext);
        if (!_tokenService.IsTokenValid(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        var isSuccess = await _ProductService.UpdateProductAsync(updateProductDto, token);
        if (isSuccess)
        {
            return RedirectToAction("Index");
        }
        return View(updateProductDto);
    }

    [Route("Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var token = _tokenService.GetTokenFromCookies(HttpContext);
        if (!_tokenService.IsTokenValid(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        var isSuccess = await _ProductService.DeleteProductAsync(id, token);
        if (isSuccess)
        {
            return RedirectToAction("Index");
        }
        return View();
    }
}
