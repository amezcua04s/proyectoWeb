using Microsoft.AspNetCore.Mvc;

namespace hospitalAPI.Controllers
{
    public class AccountController : Controller
    {
        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string usuario, string password)
        {
            // Implementa tu lógica de autenticación aquí
            if (usuario == "admin" && password == "123") // Ejemplo básico
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Credenciales inválidas");
            return View();
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(
            string nombre,
            string paterno,
            string materno,
            DateTime nacimiento,
            string genero,
            string correo,
            string confirmar,
            string password,
            string passCon)
        {
            // Implementa tu lógica de registro aquí
            if (password != passCon)
            {
                ModelState.AddModelError("passCon", "Las contraseñas no coinciden");
                return View();
            }

            // Guardar usuario en la base de datos, etc.

            return RedirectToAction("Index", "Home");
        }
    }
}