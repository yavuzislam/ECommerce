using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Payment.EntityLayer.Concrete;

namespace Payment.WebUI.ViewComponents.UILayoutViewComponents;

public class _MiniCartUILayoutComponentPartial : ViewComponent
{
    private readonly UserManager<AppUser> _userManager;

    public _MiniCartUILayoutComponentPartial(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var isAuth = HttpContext.User.Identity.IsAuthenticated;
        ViewBag.IsAuth = isAuth;
        return View();
    }
}
