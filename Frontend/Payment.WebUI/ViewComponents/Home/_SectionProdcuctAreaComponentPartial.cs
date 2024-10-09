using Microsoft.AspNetCore.Mvc;
using Payment.WebUI.Services.ProductServices;
using Payment.WebUI.Services.TokenServices;

namespace Payment.WebUI.ViewComponents.Home;

public class _SectionProdcuctAreaComponentPartial : ViewComponent
{
    private readonly IProductService _productService;
    private readonly ITokenService _tokenService;

    public _SectionProdcuctAreaComponentPartial(IProductService productService, ITokenService tokenService)
    {
        _productService = productService;
        _tokenService = tokenService;
    }

    public async Task< IViewComponentResult> InvokeAsync()
    {
        var token = _tokenService.GetTokenFromCookies(HttpContext);
        //if (!_tokenService.IsTokenValid(token))
        //{
        //    return View();
        //}

        var categories = await _productService.GetAllCategoriesByUserEmailWithCategoryNameAsync(token);
        return View(categories);
    }
}
