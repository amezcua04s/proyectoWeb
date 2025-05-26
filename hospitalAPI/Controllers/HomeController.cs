using Microsoft.AspNetCore.Mvc;

namespace hospitalAPI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Especialidades()
        {
            return View();
        }
    }
}