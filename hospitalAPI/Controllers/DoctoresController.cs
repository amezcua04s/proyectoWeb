using hospitalAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hospitalAPI.Models;

namespace hospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctoresController : ControllerBase
    {
        private readonly ClinicaDbContext _context;

        public DoctoresController(ClinicaDbContext context)
        {
            _context = context;
        }

        // GET: api/Doctores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctores()
        {
            return await _context.Doctores.ToListAsync();
        }

        // GET: api/Doctores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetDoctor(int id)
        {
            var doctor = await _context.Doctores.FindAsync(id);
            if (doctor == null) return NotFound();

            return doctor;
        }

        // POST: api/Doctores
        [HttpPost]
        public async Task<ActionResult<Doctor>> PostDoctor([FromBody] Doctor doctor)
        {
            _context.Doctores.Add(doctor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.idDoctor }, doctor);
        }

        // PUT: api/Doctores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDoctor(int id, [FromBody] Doctor doctor)
        {
            if (id != doctor.idDoctor) return BadRequest("El ID del doctor no coincide.");

            _context.Entry(doctor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Doctores.Any(a => a.idDoctor == id))
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
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctores.FindAsync(id);
            if (doctor == null) return NotFound();

            _context.Doctores.Remove(doctor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }

}