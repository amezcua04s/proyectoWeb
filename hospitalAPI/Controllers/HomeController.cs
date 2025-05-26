using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace hospitalAPI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.IsLoggedIn = HttpContext.Session.GetString("UserName") != null;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}