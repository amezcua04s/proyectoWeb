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
        private readonly List<string> especialidadesExistentes;

        public EspecialidadController(ClinicaAppDbContext context, IWebHostEnvironment env, UserManager<ClinicaUser> userManager) {

            _context = context;
            _env = env;
            _userManager = userManager;
        
        }
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
                model.Especialidades = await _context.Especialidades
                    .Select(e => e.Nombre)
                    .ToListAsync();

                return View(model);
            }

            // Validar que no exista una especialidad con el mismo nombre (ignorando mayúsculas/minúsculas)
            var existe = await _context.Especialidades
                .AnyAsync(e => e.Nombre.ToLower() == model.Nombre.Trim().ToLower());

            if (existe)
            {
                ModelState.AddModelError("Nombre", "Ya existe una especialidad con ese nombre.");
                model.Especialidades = await _context.Especialidades
                    .Select(e => e.Nombre)
                    .ToListAsync();

                return View(model);
            }

            var especialidad = new Especialidad
            {
                Nombre = model.Nombre.Trim(),
                Descripcion = model.Descripcion?.Trim()
            };

            _context.Add(especialidad);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(IndexAdmin));
        }


        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var especialidad = await _context.Especialidades.FindAsync(id);
            if (especialidad == null) return NotFound();

            var model = new EspecialidadEditViewModel
            {
                Id = especialidad.Id,
                Nombre = especialidad.Nombre,
                Descripcion = especialidad.Descripcion
            };

            return View(model);
        }



        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EspecialidadEditViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid) return View(model);

            var especialidadDb = await _context.Especialidades.FindAsync(id);
            if (especialidadDb == null) return NotFound();

            especialidadDb.Nombre = model.Nombre;
            especialidadDb.Descripcion = model.Descripcion;

            _context.Update(especialidadDb);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(IndexAdmin));
        }



        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var especialidad = await _context.Especialidades
                .Include(e => e.MedicoEspecialidades)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (especialidad == null)
                return NotFound();

            // Verifica si hay médicos vinculados antes de eliminar
            if (especialidad.MedicoEspecialidades != null && especialidad.MedicoEspecialidades.Any())
            {
                TempData["Error"] = "No se puede eliminar la especialidad porque tiene médicos asociados.";
                return RedirectToAction(nameof(IndexAdmin));
            }

            _context.Especialidades.Remove(especialidad);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Especialidad eliminada correctamente.";
            return RedirectToAction(nameof(IndexAdmin));
        }

    }
}
