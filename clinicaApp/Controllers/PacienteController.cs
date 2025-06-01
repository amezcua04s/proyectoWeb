using clinicaApp.Models;
using clinicaApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

/*
 Vistas del paciente
1 .Todos los doctores, Index Medicos
2 . crear una nueva cita
3 . ver su expediente (consulta simple, no lo puede modificar) y puede ver las citas futuras
4 . cambiar datos erroneos (editar)
5 .cambiar contraseña
 
 */
namespace clinicaApp.Controllers
{
    public class PacienteController : Controller
    {
        private readonly ClinicaAppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ClinicaUser> _userManager;

        public PacienteController(ClinicaAppDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<ClinicaUser> userManager) {

            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;

        }

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Medico"); //Este index debe regresar hacia el index normal de los doctores
        }

        public async Task<IActionResult> CrearCita(int id) //recibe el id del medico para mostrarlo en
        {
            var medico = await _context.Medicos
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medico == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            return View(medico);
        }


        [Authorize(Roles = "Paciente")]
        public async Task<IActionResult> Expediente()
        {
            var userId = _userManager.GetUserId(User);

            var paciente = await _context.Pacientes
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (paciente == null)
                return NotFound();

            var citas = await _context.Citas
                .Include(c => c.Medico)
                    .ThenInclude(m => m.User)
                .Where(c => c.PacienteId == paciente.Id.ToString())
                .ToListAsync();

            ViewBag.Citas = citas;

            return View(paciente);
        }


        public ActionResult Edit()
        {
            return View();
        } 

        public ActionResult CambiarContrasena()
        {
            return View();
        }

    }
}
