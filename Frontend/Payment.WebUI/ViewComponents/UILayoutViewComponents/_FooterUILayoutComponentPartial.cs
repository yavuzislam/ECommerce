using Microsoft.AspNetCore.Mvc;

namespace Payment.WebUI.ViewComponents.UILayoutViewComponents;

public class _FooterUILayoutComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
