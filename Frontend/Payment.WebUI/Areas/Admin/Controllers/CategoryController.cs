using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.WebUI.DTOs.CategoryDtos;
using Payment.WebUI.Services.CategoryServices;
using Payment.WebUI.Services.TokenServices;

namespace Payment.WebUI.Areas.Admin.Controllers;

[Authorize(Roles = "Admin")]
[Area("Admin")]
[Route("Admin/Category")]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly ITokenService _tokenService;

    public CategoryController(ICategoryService categoryService, ITokenService tokenService)
    {
        _categoryService = categoryService;
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

        var categories = await _categoryService.GetAllCategoriesByUserEmailAsync(token);
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
    public async Task<IActionResult> Create(CreateCategoryDto createCategoryDto)
    {
        var token = _tokenService.GetTokenFromCookies(HttpContext);
        if (!_tokenService.IsTokenValid(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        var isSuccess = await _categoryService.CreateCategoryAsync(createCategoryDto, token);
        if (isSuccess)
        {
            return RedirectToAction("Index");
        }
        return View(createCategoryDto);
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

        var category = await _categoryService.GetCategoryByIdAsync(id, token);
        return View(category);
    }

    [HttpPost]
    [Route("Update")]
    public async Task<IActionResult> Update(UpdateCategoryDto updateCategoryDto)
    {
        var token = _tokenService.GetTokenFromCookies(HttpContext);
        if (!_tokenService.IsTokenValid(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        var isSuccess = await _categoryService.UpdateCategoryAsync(updateCategoryDto, token);
        if (isSuccess)
        {
            return RedirectToAction("Index");
        }
        return View(updateCategoryDto);
    }

    [Route("Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var token = _tokenService.GetTokenFromCookies(HttpContext);
        if (!_tokenService.IsTokenValid(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        var isSuccess = await _categoryService.DeleteCategoryAsync(id, token);
        if (isSuccess)
        {
            return RedirectToAction("Index");
        }
        return View();
    }
}
