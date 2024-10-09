using Microsoft.AspNetCore.Mvc;

namespace Payment.WebUI.ViewComponents.UILayoutViewComponents;

public class _ScriptUILayoutComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
