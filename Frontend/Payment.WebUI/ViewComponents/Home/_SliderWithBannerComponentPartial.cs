using Microsoft.AspNetCore.Mvc;

namespace Payment.WebUI.ViewComponents.Home;

public class _SliderWithBannerComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
