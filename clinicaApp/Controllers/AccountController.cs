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

        public AccountController(SignInManager<ClinicaUser> signInManager, UserManager<ClinicaUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
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

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}