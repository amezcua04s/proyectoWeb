using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;

namespace hospitalAPI.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string usuario, string password, bool recordar)
        {
            // Simulación de credenciales para ADMINISTRADOR
            if (usuario == "admin@hospital.com" && password == "admin123")
            {
                HttpContext.Session.SetString("UserName", "Administrador"); // Nombre de usuario para admin
                return RedirectToAction("Index", "Home");
            }
            // Simulación de credenciales para PACIENTE (puedes cambiar "paciente@hospital.com" y "paciente123" por lo que quieras)
            else if (usuario == "paciente@hospital.com" && password == "paciente123")
            {
                HttpContext.Session.SetString("UserName", "PacienteDemo"); // Nombre de usuario para el paciente
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                return View();
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
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
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(paterno) || string.IsNullOrEmpty(correo) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(passCon))
            {
                ModelState.AddModelError(string.Empty, "Por favor, complete todos los campos requeridos.");
                return View();
            }

            if (password != passCon)
            {
                ModelState.AddModelError(string.Empty, "Las contraseñas no coinciden.");
                return View();
            }

            if (correo != confirmar)
            {
                ModelState.AddModelError(string.Empty, "Los correos electrónicos no coinciden.");
                return View();
            }

            if (!correo.Contains("@") || !correo.Contains("."))
            {
                ModelState.AddModelError(string.Empty, "Formato de correo electrónico inválido.");
                return View();
            }

            if (password.Length < 6)
            {
                ModelState.AddModelError(string.Empty, "La contraseña debe tener al menos 6 caracteres.");
                return View();
            }

            // Simulación de registro exitoso.
            // Aquí podrías guardar el 'nombre' del usuario en un TempData para mostrarlo en el login,
            // pero como no hay DB, no lo "recordaremos" de forma persistente.
            TempData["SuccessMessage"] = $"¡Registro exitoso para {nombre}! Ahora puedes 'iniciar sesión' con '{correo}' y la contraseña que ingresaste (solo para fines de prueba).";

            // Redirige al login. No establecemos la sesión aquí porque el usuario debe "loguearse" después de registrarse.
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult CambioContrasenia()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]
        public IActionResult CambioContrasenia(string oldPassword, string newPassword, string confirmNewPassword)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (newPassword != confirmNewPassword)
            {
                ModelState.AddModelError(string.Empty, "La nueva contraseña y la confirmación no coinciden.");
                return View();
            }

            // Simulación de cambio de contraseña exitoso.
            TempData["SuccessMessage"] = "Tu contraseña ha sido cambiada. (Simulado)";
            return RedirectToAction("Perfil", "Paciente"); // Mejor redirigir al perfil del paciente
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}