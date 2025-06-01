using clinicaApp.Models;
using clinicaApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using clinicaApp.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace clinicaApp.Controllers
{
    /* VISTAS PARA ESPECIALIDAD
     * IndexAdmin <- solo la verá el admin y será el listado
     * Index para que el usuario pueda ver todas las especialidades
     * 
     * Create
     * Edit
     */
    public class EspecialidadController : Controller
    {
        private readonly ClinicaAppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<ClinicaUser> _userManager;

        public EspecialidadController(ClinicaAppDbContext context, IWebHostEnvironment env, UserManager<ClinicaUser> userManager) {

            _context = context;
            _env = env;
            _userManager = userManager;
        
        }
        //Vista donde se verán todas las especialidades y el usuario poodrá buscar medico segun la especialidad que quiewra
        public async Task<IActionResult> Index() {
            var especialidades = await _context.Especialidades.ToListAsync();
            return View(especialidades);
        }
        //Vista donde se ven todas, el index que solo verá el admin
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> IndexAdmin()
        {
            var especialidades = await _context.Especialidades.ToListAsync();
            return View(especialidades);
        }

        // Llama a la vista de crear una nueva vista
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create()
        {
            var model = new EspecialidadViewModel
            {
                Especialidades = await _context.Especialidades
                    .Select(e => e.Nombre.ToLower())
                    .ToListAsync()
            };

            return View(model);
        }



        // Intenta poner en la base de datos la nueva especialidad
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(EspecialidadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var kvp in ModelState)
                {
                    foreach (var error in kvp.Value.Errors)
                    {
                        Console.WriteLine($"Campo: {kvp.Key}, Error: {error.ErrorMessage}");
                    }
                }

                model.Especialidades = _context.Especialidades
                    .Select(e => e.Nombre.ToLower())
                    .ToList();

                return View(model);
            }

            var especialidad = new Especialidad
            {
                Nombre = model.Nombre,
                Descripcion = model.Descripcion
            };

            _context.Add(especialidad);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(IndexAdmin));
        }


        // GET: EspecialidadController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EspecialidadController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EspecialidadController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EspecialidadController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
