using clinicaApp.Models;
using clinicaApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using clinicaApp.ViewModels;


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

        //Dos vistas de index para admin, muestra los medicos activos y todos los inactivos

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> IndexAdmin()
        {
            var medicos = await _context.Medicos
                .Include(m => m.User)
                .Where(m => m.User.Activo == true) // <-- Solo medicos activos
                .ToListAsync();

            return View(medicos);
        }

        /*
         muestra los que estan dados de baja
         */
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> IndexBaja()
        {
            var medicosBaja = await _context.Medicos
                .Include(m => m.User)
                .Where(m => m.User.Activo == false)
                .ToListAsync();

            return View(medicosBaja);
        }


        /*
         Solo muestra lo escencial del médico, no incluye la información del usuario.
         */
        [AllowAnonymous]
        public async Task<IActionResult> Index() {
            var medicos = await _context.Medicos
                .Include(m => m.User)
                .Where(m => m.User.Activo == true)//Incluir solamente los que tienen el estado activo como true
                .Select(m => new Medico
                {
                    Id = m.Id,
                    UserId = m.UserId,
                    Especialidad = m.Especialidad,
                    CedulaProfesional = m.CedulaProfesional,
                    Foto = m.Foto,
                    //Disponibilidades = m.Disponibilidades,

                    User = new ClinicaUser
                    {
                        Nombre = m.User.Nombre,
                        Materno = m.User.Materno,
                        Paterno = m.User.Paterno,
                        Sexo = m.User.Sexo,
                        Correo = m.User.Correo,
                        Contrasenia = m.User.Contrasenia,
                        Telefono = m.User.Telefono,

                    },
                })
                .ToListAsync();
            return View(medicos);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Create() => View(new MedicoViewModel());

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(MedicoViewModel model)
        {
            if (ModelState.IsValid)
            {
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

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Doctor");

                    var medico = new Medico
                    {
                        User = user,
                        Nacimiento = model.Nacimiento,
                        Especialidad = model.Especialidad,
                        CedulaProfesional = model.CedulaProfesional
                    };

                    // Manejo de imagen
                    if (model.Foto != null)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(model.Foto.FileName);
                        var path = Path.Combine(_env.WebRootPath, "images", "medicos");
                        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                        var uploadImage = Path.Combine(path, fileName);

                        using (var stream = new FileStream(uploadImage, FileMode.Create))
                        {
                            await model.Foto.CopyToAsync(stream);
                        }

                        medico.Foto = "/images/medicos/" + fileName;
                    }
                    else
                    {
                        medico.Foto = "/images/default_doctor.png";
                    }

                    _context.Add(medico);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }


        /*
         EDITAR
        Recibe el id del doctor a editar y muestra el formulario de edición.
         */
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id)
        {
            var medico = await _context.Medicos
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medico == null) return NotFound();

            var model = new MedicoEditViewModel
            {
                Id = medico.Id,
                Nombre = medico.User.Nombre,
                Paterno = medico.User.Paterno,
                Materno = medico.User.Materno,
                Telefono = medico.User.Telefono,
                Correo = medico.User.Correo,
                Sexo = medico.User.Sexo,
                Especialidad = medico.Especialidad,
                CedulaProfesional = medico.CedulaProfesional,
                Nacimiento = medico.Nacimiento,
                FotoActual = medico.Foto
            };

            return View(model);
        }


        //Maneja la edición del médico.
        //CUANDO SE DA SUBMIT AL FORMULARIO DE EDICIÓN, SE ENVÍA UNA SOLICITUD POST CON LOS DATOS DEL MÉDICO.
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(MedicoEditViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var medicoDb = await _context.Medicos
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == model.Id);

            if (medicoDb == null) return NotFound();

            // Actualiza datos del usuario
            medicoDb.User.Nombre = model.Nombre;
            medicoDb.User.Paterno = model.Paterno;
            medicoDb.User.Materno = model.Materno;
            medicoDb.User.Telefono = model.Telefono;
            medicoDb.User.Correo = model.Correo;
            medicoDb.User.Sexo = model.Sexo;

            // Datos del médico
            medicoDb.Especialidad = model.Especialidad;
            medicoDb.CedulaProfesional = model.CedulaProfesional;
            medicoDb.Nacimiento = model.Nacimiento;

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

            _context.Update(medicoDb);
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

