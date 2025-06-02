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

        private static readonly Dictionary<string, Dias> MapeoDias = new()
        {
            { "Lunes", Dias.LUNES },
            { "Martes", Dias.MARTES },
            { "Miércoles", Dias.MIERCOLES },
            { "Jueves", Dias.JUEVES },
            { "Viernes", Dias.VIERNES },
            { "Sábado", Dias.SABADO },
            { "Domingo", Dias.DOMINGO }
        };


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
            };

            _context.Medicos.Add(medico);
            await _context.SaveChangesAsync(); //Guarda el medico en la BD para generarle un ID

            var disponibilidades = ProcesarDisponibilidades(model.DisponibilidadesPorDia, medico.Id);
            _context.Disponibilidades.AddRange(disponibilidades);
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
                .Include(m => m.Disponibilidades)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medico == null) return NotFound();

            // Agrupar por día, y para cada día generar texto: "08:00-12:00,16:00-18:00"
            var disponibilidadesPorDia = medico.Disponibilidades
                .GroupBy(d => d.DiaDeLaSemana)
                .ToDictionary(
                    g => g.Key.ToString().ToUpper()[..1] + g.Key.ToString().ToLower()[1..], // Ej: "Lunes"
                    g => string.Join(",", g.OrderBy(d => d.HoraInicio)
                                          .Chunk(1)
                                          .Select(c => $"{c.First().HoraInicio:hh\\:mm}-{c.First().HoraFin:hh\\:mm}"))
                );

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
                DisponibilidadesPorDia = disponibilidadesPorDia
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
            // Eliminar disponibilidades antiguas
            var disponibilidadesActuales = await _context.Disponibilidades
                .Where(d => d.MedicoId == medicoDb.Id)
                .ToListAsync();
            _context.Disponibilidades.RemoveRange(disponibilidadesActuales);

            // Agregar nuevas
            var nuevasDisponibilidades = ProcesarDisponibilidades(model.DisponibilidadesPorDia, medicoDb.Id);
            _context.Disponibilidades.AddRange(nuevasDisponibilidades);


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
        private List<Disponibilidad> ProcesarDisponibilidades(Dictionary<string, string> horarios, int medicoId)
        {
            var lista = new List<Disponibilidad>();

            foreach (var entrada in horarios)
            {
                if (string.IsNullOrWhiteSpace(entrada.Value)) continue;

                if (!MapeoDias.TryGetValue(entrada.Key, out var dia)) continue;

                var bloques = entrada.Value.Split(',', StringSplitOptions.RemoveEmptyEntries);

                foreach (var bloque in bloques)
                {
                    var partes = bloque.Split('-', StringSplitOptions.RemoveEmptyEntries);
                    if (partes.Length != 2) continue;

                    if (TimeSpan.TryParse(partes[0].Trim(), out var inicio) &&
                        TimeSpan.TryParse(partes[1].Trim(), out var fin))
                    {
                        for (var hora = inicio; hora < fin; hora = hora.Add(TimeSpan.FromHours(1)))
                        {
                            lista.Add(new Disponibilidad
                            {
                                MedicoId = medicoId,
                                DiaDeLaSemana = dia,
                                HoraInicio = hora,
                                HoraFin = hora.Add(TimeSpan.FromHours(1)),
                                EstaOcupado = false
                            });
                        }
                    }
                }
            }

            return lista;
        }

        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> VistaInicial()
        {
            

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var medico = await _context.Medicos
                .Include(m => m.Citas.Where(c => c.FechaHora >= DateTime.Now))
                    .ThenInclude(c => c.Paciente)
                        .ThenInclude(p => p.User)
                .Include(m => m.Citas)
                    .ThenInclude(c => c.Paciente)
                        .ThenInclude(p => p.Expediente)
                .FirstOrDefaultAsync(m => m.UserId == user.Id);

            if (medico == null) return NotFound();

            if (medico.primerInicio)
            {
                return RedirectToAction("ChangePassword", "Account");
            }

            var citasProximas = medico.Citas
                .Where(c => c.Estado == EstadoCita.Pendiente)
                .OrderBy(c => c.FechaHora)
                .ToList();

            return View("VistaInicial", citasProximas);
        }

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> AgregarNota(int citaId, string nota)
        {
            var cita = await _context.Citas.FirstOrDefaultAsync(c => c.Id == citaId);
            if (cita == null) return NotFound();

            cita.Notas = nota;
            await _context.SaveChangesAsync();

            return RedirectToAction("VistaInicial");
        }

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> ActualizarExpediente(int pacienteId, string alergia, string medicamento, string enfermedad, string tratamiento)
        {
            var paciente = await _context.Pacientes
                .Include(p => p.Expediente)
                .FirstOrDefaultAsync(p => p.Id == pacienteId);

            if (paciente == null) return NotFound();

            // Crear expediente si no tiene
            if (paciente.Expediente == null)
            {
                paciente.Expediente = new Expediente
                {
                    PacienteId = paciente.Id,
                    FechaCreacion = DateTime.Now,
                    FechaModificacion = DateTime.Now
                };
                _context.Expedientes.Add(paciente.Expediente);
            }
            else
            {
                paciente.Expediente.FechaModificacion = DateTime.Now;
            }

            if (!string.IsNullOrWhiteSpace(alergia) && !paciente.Alergias.Contains(alergia))
                paciente.Alergias.Add(alergia);

            if (!string.IsNullOrWhiteSpace(medicamento) && !paciente.Medicamentos.Contains(medicamento))
                paciente.Medicamentos.Add(medicamento);

            if (!string.IsNullOrWhiteSpace(enfermedad) && !paciente.Enfermedades.Contains(enfermedad))
                paciente.Enfermedades.Add(enfermedad);

            if (!string.IsNullOrWhiteSpace(tratamiento) && !paciente.Tratamientos.Contains(tratamiento))
                paciente.Tratamientos.Add(tratamiento);

            await _context.SaveChangesAsync();
            return RedirectToAction("VistaInicial", "Medico");
        }


    }
}
