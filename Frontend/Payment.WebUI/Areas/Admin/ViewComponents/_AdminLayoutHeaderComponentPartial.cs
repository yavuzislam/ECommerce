using Microsoft.AspNetCore.Mvc;

namespace Payment.WebUI.Areas.Admin.ViewComponents;

public class _AdminLayoutHeaderComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
