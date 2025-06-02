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
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var medico = await _context.Medicos
                .Include(m => m.User)
                .Include(m => m.Disponibilidades)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medico == null) return NotFound();


            return View(medico);
        }

        [HttpPost]
        [Authorize(Roles = "Paciente")]
        public async Task<IActionResult> CitaCreation(int medicoId, string horario)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.UserId == user.Id);
            if (paciente == null) return NotFound("Paciente no encontrado");

            // Parseo del string enviado (Ej: "Lunes 08:00")
            var partes = horario.Split(' ');
            if (partes.Length != 2 || !Enum.TryParse(partes[0], out Dias dia) || !TimeSpan.TryParse(partes[1], out TimeSpan hora))
                return BadRequest("Horario inválido");

            var disponibilidad = await _context.Disponibilidades
                .FirstOrDefaultAsync(d =>
                    d.MedicoId == medicoId &&
                    d.DiaDeLaSemana == dia &&
                    d.HoraInicio == hora &&
                    !d.EstaOcupado);

            if (disponibilidad == null)
                return BadRequest("El horario ya no está disponible.");

            // Marcamos el horario como ocupado
            disponibilidad.EstaOcupado = true;

            // Calculamos la próxima fecha con base en el día de la semana
            var hoy = DateTime.Today;
            int diasHasta = ((int)dia - (int)hoy.DayOfWeek + 7) % 7;
            var fechaCita = hoy.AddDays(diasHasta).Add(hora);

            var cita = new Cita
            {
                PacienteId = paciente.Id,
                MedicoId = medicoId,
                FechaHora = fechaCita,
                Motivo = "Consulta general", // puedes luego dejarlo configurable
                Estado = EstadoCita.Pendiente,
                Notas = ""
            };

            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();

            return RedirectToAction("CitasProgramadas", "Paciente");
        }

        [Authorize(Roles = "Paciente")]
        public async Task<IActionResult> CitasProgramadas()
        {
            var userId = _userManager.GetUserId(User);

            var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.UserId == userId);
            if (paciente == null) return NotFound();

            var citas = await _context.Citas
                .Include(c => c.Medico)
                .ThenInclude(m => m.User)
                .Where(c => c.PacienteId == paciente.Id)
                .OrderBy(c => c.FechaHora)
                .ToListAsync();

            return View(citas);
        }

        [HttpPost]
        [Authorize(Roles = "Paciente")]
        public async Task<IActionResult> CancelarCita(int id)
        {
            var cita = await _context.Citas
                .Include(c => c.Medico)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cita == null) return NotFound();

            cita.Estado = EstadoCita.Cancelada;

            // Liberar disponibilidad si coincide con un bloque
            var disponibilidad = await _context.Disponibilidades.FirstOrDefaultAsync(d =>
                d.MedicoId == cita.MedicoId &&
                d.HoraInicio == cita.FechaHora.TimeOfDay &&
                d.DiaDeLaSemana == (Dias)cita.FechaHora.DayOfWeek);

            if (disponibilidad != null)
            {
                disponibilidad.EstaOcupado = false;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("CitasProgramadas");
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
                .Where(c => c.PacienteId == paciente.Id)
                .ToListAsync();

            ViewBag.Citas = citas;

            return View(paciente);
        }


        public ActionResult Edit()
        {
            return View();
        }

        public async Task<IActionResult> CambiarContrasena()
        {
            return RedirectToAction("ChangePassword", "Account");
        }

    }
}
