using clinicaApp.Data;
using clinicaApp.Models;
using clinicaApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace clinicaApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ClinicaUser> _signInManager;
        private readonly UserManager<ClinicaUser> _userManager;
        private readonly ClinicaAppDbContext _context;

        public AccountController(ClinicaAppDbContext context ,SignInManager<ClinicaUser> signInManager, UserManager<ClinicaUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Administrador"))
                {

                    return RedirectToAction("Index", "Admin");

                } else if (roles.Contains("Doctor")) {

                    return RedirectToAction("Index", "Doctor");

                }
                    //si no es administrador ni doctor por el momento lo llevamos al home normal
                    return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Credenciales inválidas");
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {
            if (!ModelState.IsValid)
                return View(registerModel);

            if (!registerModel.Correo.Equals(registerModel.ConfirmCorreo)) {
                return View(registerModel);
            }

            var user = new ClinicaUser
            {
                UserName = registerModel.Correo,
                Email = registerModel.Correo,
                Nombre = registerModel.Nombre,
                Paterno = registerModel.Paterno,
                Materno = registerModel.Materno,
                Telefono = registerModel.Telefono,
                Sexo = registerModel.Sexo,
                Activo = true
            };

            var result = await _userManager.CreateAsync(user, registerModel.Contrasenia);

            if (result.Succeeded)
            {
                // Asigna rol
                await _userManager.AddToRoleAsync(user, "Paciente");

                var paciente = new Paciente
                {
                    UserId = user.Id,
                    User = user,
                    Nacimiento = registerModel.Nacimiento,
                    Alergias = Request.Form["Alergias"].ToString().Split(',').Select(a => a.Trim()).ToList(),
                    Enfermedades = Request.Form["Enfermedades"].ToString().Split(',').Select(a => a.Trim()).ToList(),
                    Medicamentos = Request.Form["Medicamentos"].ToString().Split(',').Select(a => a.Trim()).ToList(),
                    Antecedentes = Request.Form["Antecedentes"].ToString().Split(',').Select(a => a.Trim()).ToList(),
                    Cirugias = Request.Form["Cirugias"].ToString().Split(',').Select(a => a.Trim()).ToList(),
                    Tratamientos = Request.Form["Tratamientos"].ToString().Split(',').Select(a => a.Trim()).ToList()
                };


                _context.Pacientes.Add(paciente);
                await _context.SaveChangesAsync();

                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(registerModel);
        }


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}