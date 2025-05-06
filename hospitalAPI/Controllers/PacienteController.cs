using hospitalAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hospitalAPI.Models;
namespace hospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacienteController : Controller
    {
        private readonly ClinicaDbContext _context;

        public PacienteController(ClinicaDbContext context)
        {
            _context = context;
        }

        // GET: PacienteController
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Paciente>>> GetPacientes()
        {
            return await _context.Pacientes.ToListAsync();
        }

        // GET: PacienteController/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Paciente>> GetPaciente(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null) return NotFound();

            return paciente;
        }

        // GET: PacienteController/Create
        [HttpPost]
        public async Task<ActionResult<Paciente>> PostPaciente([FromBody] Paciente paciente)
        {
            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPaciente), new { id = paciente.idPaciente }, paciente);
        }

        // PUT: api/Doctores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDoctor(int id, [FromBody] Paciente paciente)
        {
            if (id != paciente.idPaciente) return BadRequest("El ID del doctor no coincide.");

            _context.Entry(paciente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Pacientes.Any(a => a.idPaciente == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Doctores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaciente(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null) return NotFound();

            _context.Pacientes.Remove(paciente);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
