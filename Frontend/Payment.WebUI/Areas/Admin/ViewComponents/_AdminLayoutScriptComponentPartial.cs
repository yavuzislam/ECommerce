using Microsoft.AspNetCore.Mvc;

namespace Payment.WebUI.Areas.Admin.ViewComponents;

public class _AdminLayoutScriptComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
