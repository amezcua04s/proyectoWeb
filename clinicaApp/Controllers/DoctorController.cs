using clinicaApp.Models;
using clinicaApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace clinicaApp.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ClinicaAppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductosController(ClinicaAppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Medicos.ToListAsync());
        }
        [Authorize(Roles = "Administrador")]
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(Medico medico, IFormFile Foto)
        {
            if (ModelState.IsValid)
            {
                if (Foto!= null)
                {
                    var fileName = Path.GetFileName(Foto.FileName);
                    var path = Path.Combine(_env.WebRootPath, "images", fileName);
                    using var stream = new FileStream(path, FileMode.Create);
                    await Foto.CopyToAsync(stream);
                    medico.Foto = "/images/" + fileName;
                }

                _context.Add(medico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medico);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var medico = await _context.Medicos.FindAsync(id);
            return medico == null ? NotFound() : View(medico);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id, Medico medico, IFormFile RutaImagen)
        {
            if (id != medico.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (RutaImagen != null)
                    {
                        var fileName = Path.GetFileName(RutaImagen.FileName);
                        var path = Path.Combine(_env.WebRootPath, "images", fileName);
                        using var stream = new FileStream(path, FileMode.Create);
                        await RutaImagen.CopyToAsync(stream);
                        medico.Foto = "/images/" + fileName;
                    }
                    _context.Update(medico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Medicos.Any(m => m.Id == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(medico);
        }

        [AllowAnonymous] // Permite acceso sin login
        public IActionResult IndexPublico()
        {
            var medicos = _context.Medicos.ToList();
            return View(medicos);
        }
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var medico = await _context.Medicos.FindAsync(id);
            if (medico != null)
            {
                _context.Medicos.Remove(medico);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }

}

