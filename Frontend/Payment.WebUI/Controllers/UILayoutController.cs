using Microsoft.AspNetCore.Mvc;

namespace Payment.WebUI.Controllers;

public class UILayoutController : Controller
{
    public IActionResult _UILayout()
    {
        return View();
    }
}
