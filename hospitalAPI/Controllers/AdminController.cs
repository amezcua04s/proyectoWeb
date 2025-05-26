using Microsoft.AspNetCore.Mvc;

namespace hospitalAPI.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Admin()
        {
            return View("Admin");
        }

        public IActionResult AgregarDoctor()
        {
            return View();
        }
    }
}
