using Microsoft.AspNetCore.Mvc;
using Payment.WebUI.Services.AuthServices;

namespace Payment.WebUI.Controllers;

public class HomeController : Controller
{
    private readonly IAuthService _authService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HomeController(IAuthService authService, IHttpContextAccessor httpContextAccessor)
    {
        _authService = authService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task <IActionResult> Index()
    {
        var token1=_authService.GetTokenCookie();
        var token=_httpContextAccessor.HttpContext.Request.Cookies["jwtToken"];
        if (!User.Identity.IsAuthenticated&&!string.IsNullOrEmpty(token))
        {
            _authService.RemoveTokenCookie();
        }
        return View();
    }
}
