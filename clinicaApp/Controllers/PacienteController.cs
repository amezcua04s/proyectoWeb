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
3 . ver su expediente (consulta simple, no lo puede modificar)
4 . cambiar datos erroneos (editar)
5 .cambiar contraseña
6 . consultar citas, proximas y pasadas (todas en la misma lista)
 
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

        public ActionResult CrearCita()
        {
            return View();
        }

        public ActionResult Expediente() {
            return View();
        }
    }
}
