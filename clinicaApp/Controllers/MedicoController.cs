using clinicaApp.Models;
using clinicaApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace clinicaApp.Controllers
{
    public class MedicoController : Controller
    {
        private readonly ClinicaAppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<ClinicaUser> _userManager; // <--- Sin el namespace completo aquí


        public MedicoController(ClinicaAppDbContext context, IWebHostEnvironment env, UserManager<ClinicaUser> userManager) // <--- Aquí también, solo el tipo
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }



        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> IndexAdmin()
        {
            var medicos = await _context.Medicos
                .Include(m => m.User) // Incluye la información del usuario asociado al médico
                .ToListAsync();
            return View(await _context.Medicos.ToListAsync());
        }

        /*
         Solo muestra lo escencial del médico, no incluye la información del usuario.
         */
        [AllowAnonymous] // Permite acceso sin login, va a ser un preview de los medicos disponibles
        public async Task<IActionResult> Index() {
            var medicos = await _context.Medicos
                .Include(m=> m.User)
                .Select(m => new Medico
                {
                    Id = m.Id,
                    UserId = m.UserId,
                    Especialidad = m.Especialidad,
                    CedulaProfesional = m.CedulaProfesional,
                    Telefono = m.Telefono,
                    Foto = m.Foto,
                    Disponibilidades = m.Disponibilidades,
                    

                    User = new ClinicaUser
                    {
                        Nombre = m.User.Nombre,
                        Materno = m.User.Materno,
                        Paterno = m.User.Paterno,
                        Sexo = m.User.Sexo,

                    },
                })
                .ToListAsync();
            return View(medicos);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(Medico medico, IFormFile Foto)
        {
            if (ModelState.IsValid)
            {
                // Crear el nuevo usuario
                var user = new ClinicaUser { 
                    Nombre = medico.User.Nombre,
                    Paterno = medico.User.Paterno,
                    Materno = medico.User.Materno,
                    Telefono = medico.User.Telefono,
                    Sexo = medico.User.Sexo,
                    UserName = medico.User.Correo, 
                    Email = medico.User.Correo };
                var result = await _userManager.CreateAsync(user, medico.User.Contrasenia);

                if (result.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "Doctor");

                    if (!roleResult.Succeeded)
                    {
                        // Aquí, simplemente agregamos los errores al ModelState
                        foreach (var error in roleResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, $"Error al asignar rol: {error.Description}");
                        }
                        // Si no se puede asignar el rol, es posible que quieras eliminar el usuario que acabas de crear
                        await _userManager.DeleteAsync(user);
                        return View(medico); // Regresar a la vista con el error
                    }

                    // Asignar el UserId del usuario recién creado al Medico
                    medico.UserId = user.Id;

                    // Manejar la subida de la foto
                    if (Foto != null)
                    {
                        var fileName = Path.GetFileName(Foto.FileName);
                        var uploadFolder = Path.Combine(_env.WebRootPath, "images", "medicos");
                        if (!Directory.Exists(uploadFolder))
                        {
                            Directory.CreateDirectory(uploadFolder);
                        }
                        var path = Path.Combine(uploadFolder, fileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await Foto.CopyToAsync(stream);
                        }
                        medico.Foto = "/images/medicos/" + fileName;
                    }
                    else
                    {
                        medico.Foto = "/images/default_doctor.png"; // Imagen por defecto
                    }

                    // 5. Guardar el perfil del médico
                    _context.Add(medico);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index)); // Redirigir a la lista de médicos
                }
                else
                {
                    // Si la creación del usuario falla, agregar los errores al ModelState
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // Si el ModelState no es válido o si la creación del usuario/rol falló,
            // regresa a la vista con los errores.
            return View(medico);
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

            return medico == null ? NotFound() : View(medico);
        }

        //Maneja la edición del médico.
        //CUANDO SE DA SUBMIT AL FORMULARIO DE EDICIÓN, SE ENVÍA UNA SOLICITUD POST CON LOS DATOS DEL MÉDICO.
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id, Medico medico, IFormFile RutaImagen)
        {
            if (id != medico.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var medicoDb = await _context.Medicos.FindAsync(id);
                    if (medicoDb == null) return NotFound();

                    medicoDb.Especialidad = medico.Especialidad;
                    medicoDb.CedulaProfesional = medico.CedulaProfesional;
                    medicoDb.Notas = medico.Notas;
                    medicoDb.Telefono = medico.Telefono;
                    medicoDb.User.Sexo = medico.User.Sexo;
                    medicoDb.Nacimiento = medico.Nacimiento;

                    if (RutaImagen != null)
                    {
                        var fileName = Path.GetFileName(RutaImagen.FileName);
                        var path = Path.Combine(_env.WebRootPath, "images", fileName);
                        using var stream = new FileStream(path, FileMode.Create);
                        await RutaImagen.CopyToAsync(stream);
                        medicoDb.Foto = "/images/" + fileName;
                    }

                    _context.Update(medicoDb);
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

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var medico = await _context.Medicos
                .Include(m => m.Disponibilidades)
                .Include(m => m.Citas)
                .Include(m => m.Expedientes)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medico != null)
            {
                _context.Medicos.Remove(medico);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }

}

