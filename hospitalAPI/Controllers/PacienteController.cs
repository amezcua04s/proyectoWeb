using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace hospitalAPI.Controllers
{
    public class PacienteController : Controller
    {
        public IActionResult Paciente()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View("~/Views/Home/Paciente.cshtml");
        }

        public IActionResult CitasProgramadas()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View("~/Views/Doctor/CitasProgramadas.cshtml");
        }

        public IActionResult Especialidades()
        {
            ViewBag.IsLoggedIn = HttpContext.Session.GetString("UserName") != null;
            return View("~/Views/Home/Especialidades.cshtml");
        }

        public IActionResult AgendarCita()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View("~/Views/Home/AgendarCita.cshtml");
        }

        public IActionResult Perfil()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View("~/Views/Home/Perfil.cshtml");
        }
    }
}