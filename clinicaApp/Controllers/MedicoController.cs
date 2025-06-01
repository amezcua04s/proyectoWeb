using clinicaApp.Models;
using clinicaApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using clinicaApp.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace clinicaApp.Controllers
{
    public class MedicoController : Controller
    {
        private readonly ClinicaAppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<ClinicaUser> _userManager;

        public MedicoController(ClinicaAppDbContext context, IWebHostEnvironment env, UserManager<ClinicaUser> userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> IndexAdmin()
        {
            var medicos = await _context.Medicos
                .Include(m => m.User)
                .Where(m => m.User.Activo == true)
                .ToListAsync();

            return View(medicos);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> IndexBaja()
        {
            var medicosBaja = await _context.Medicos
                .Include(m => m.User)
                .Where(m => m.User.Activo == false)
                .ToListAsync();

            return View(medicosBaja);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var medicos = await _context.Medicos
                .Include(m => m.User)
                .Include(m => m.MedicoEspecialidades)
                    .ThenInclude(me => me.Especialidad)
                .Where(m => m.User.Activo == true)
                .Select(m => new Medico
                {
                    Id = m.Id,
                    UserId = m.UserId,
                    CedulaProfesional = m.CedulaProfesional,
                    Foto = m.Foto,
                    Disponibilidades = m.Disponibilidades,
                    Especialidades = m.MedicoEspecialidades
                                        .Select(me => me.Especialidad.Nombre)
                                        .ToList(),
                    User = new ClinicaUser
                    {
                        Nombre = m.User.Nombre,
                        Materno = m.User.Materno,
                        Paterno = m.User.Paterno,
                        Sexo = m.User.Sexo,
                        Correo = m.User.Correo,
                        Contrasenia = m.User.Contrasenia,
                        Telefono = m.User.Telefono
                    }
                })
                .ToListAsync();

            return View(medicos);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            MedicoViewModel model = new MedicoViewModel();

            model.EspecialidadesDisponibles = _context.Especialidades
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.Nombre
                }).ToList();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(MedicoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.EspecialidadesDisponibles = _context.Especialidades
                    .Select(e => new SelectListItem
                    {
                        Value = e.Id.ToString(),
                        Text = e.Nombre
                    }).ToList();

                return View(model);
            }

            var user = new ClinicaUser
            {
                Nombre = model.Nombre,
                Paterno = model.Paterno,
                Materno = model.Materno,
                Sexo = model.Sexo,
                Telefono = model.Telefono,
                UserName = model.Correo,
                Email = model.Correo,
                Activo = true,
            };

            var result = await _userManager.CreateAsync(user, model.Contrasenia);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                model.EspecialidadesDisponibles = _context.Especialidades
                    .Select(e => new SelectListItem
                    {
                        Value = e.Id.ToString(),
                        Text = e.Nombre
                    }).ToList();

                return View(model);
            }

            await _userManager.AddToRoleAsync(user, "Doctor");

            var disponibilidades = model.DisponibilidadesPorDia
                .SelectMany(d => d.Value.Select(h => $"{d.Key} {h}"))
                .ToList();

            string rutaFoto = "/images/default_doctor.png";
            if (model.Foto != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(model.Foto.FileName);
                var path = Path.Combine(_env.WebRootPath, "images", "medicos");
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                var fullPath = Path.Combine(path, fileName);
                using var stream = new FileStream(fullPath, FileMode.Create);
                await model.Foto.CopyToAsync(stream);
                rutaFoto = "/images/medicos/" + fileName;
            }

            var medico = new Medico
            {
                User = user,
                Nacimiento = model.Nacimiento,
                CedulaProfesional = model.CedulaProfesional,
                Foto = rutaFoto,
                Disponibilidades = disponibilidades
            };

            _context.Medicos.Add(medico);
            await _context.SaveChangesAsync();

            if (model.EspecialidadesSeleccionadas != null && model.EspecialidadesSeleccionadas.Any())
            {
                foreach (var id in model.EspecialidadesSeleccionadas)
                {
                    _context.MedicoEspecialidades.Add(new MedicoEspecialidad
                    {
                        MedicoId = medico.Id,
                        EspecialidadId = id
                    });
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id)
        {
            var medico = await _context.Medicos
                .Include(m => m.User)
                .Include(m => m.MedicoEspecialidades)
                    .ThenInclude(me => me.Especialidad)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medico == null) return NotFound();

            var model = new MedicoEditViewModel
            {
                Id = medico.Id,
                Nombre = medico.User.Nombre,
                Paterno = medico.User.Paterno,
                Materno = medico.User.Materno,
                Telefono = medico.User.Telefono,
                Correo = medico.User.Email,
                Sexo = medico.User.Sexo,
                CedulaProfesional = medico.CedulaProfesional,
                Nacimiento = medico.Nacimiento,
                FotoActual = medico.Foto,
                EspecialidadesSeleccionadas = medico.MedicoEspecialidades
                                               .Select(me => me.EspecialidadId)
                                               .ToList(),
                EspecialidadesDisponibles = await _context.Especialidades
                    .Select(e => new SelectListItem
                    {
                        Value = e.Id.ToString(),
                        Text = e.Nombre
                    }).ToListAsync(),
                DisponibilidadesPorDia = medico.Disponibilidades
                    .GroupBy(d => d.Split(" ")[0]) // Día
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(d => d.Split(" ")[1]).ToList() // Horas
                    )
            };

            return View(model);
        }


        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(MedicoEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.EspecialidadesDisponibles = await _context.Especialidades
                    .Select(e => new SelectListItem
                    {
                        Value = e.Id.ToString(),
                        Text = e.Nombre
                    }).ToListAsync();

                return View(model);
            }

            var medicoDb = await _context.Medicos
                .Include(m => m.User)
                .Include(m => m.MedicoEspecialidades)
                .FirstOrDefaultAsync(m => m.Id == model.Id);

            if (medicoDb == null) return NotFound();

            // Actualizar datos del usuario
            medicoDb.User.Nombre = model.Nombre;
            medicoDb.User.Paterno = model.Paterno;
            medicoDb.User.Materno = model.Materno;
            medicoDb.User.Telefono = model.Telefono;
            medicoDb.User.Email = model.Correo;
            medicoDb.User.UserName = model.Correo;
            medicoDb.User.Sexo = model.Sexo;

            // Actualizar datos del médico
            medicoDb.CedulaProfesional = model.CedulaProfesional;
            medicoDb.Nacimiento = model.Nacimiento;

            // Actualizar foto si se subió una nueva
            if (model.NuevaFoto != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(model.NuevaFoto.FileName);
                var path = Path.Combine(_env.WebRootPath, "images", "medicos");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var fullPath = Path.Combine(path, fileName);
                using var stream = new FileStream(fullPath, FileMode.Create);
                await model.NuevaFoto.CopyToAsync(stream);

                medicoDb.Foto = "/images/medicos/" + fileName;
            }

            // Actualizar especialidades (eliminar y agregar)
            _context.MedicoEspecialidades.RemoveRange(medicoDb.MedicoEspecialidades);

            if (model.EspecialidadesSeleccionadas != null)
            {
                foreach (var espId in model.EspecialidadesSeleccionadas)
                {
                    _context.MedicoEspecialidades.Add(new MedicoEspecialidad
                    {
                        MedicoId = medicoDb.Id,
                        EspecialidadId = espId
                    });
                }
            }

            // Actualizar disponibilidades
            medicoDb.Disponibilidades = model.DisponibilidadesPorDia
                .SelectMany(d => d.Value.Select(h => $"{d.Key} {h}"))
                .ToList();

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexAdmin));
        }


        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var medico = await _context.Medicos
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medico?.User != null)
            {
                medico.User.Activo = false;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(IndexAdmin));
        }
    }
}
