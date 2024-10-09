using Microsoft.AspNetCore.Mvc;

namespace Payment.WebUI.ViewComponents.UILayoutViewComponents;

public class _HeaderUILayoutComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
