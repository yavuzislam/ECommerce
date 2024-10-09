using Microsoft.AspNetCore.Mvc;

namespace Payment.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Dasboard")]
    public class DashboardController : Controller
    {
        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
