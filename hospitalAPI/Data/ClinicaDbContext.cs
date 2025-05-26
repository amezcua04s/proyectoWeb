using hospitalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace hospitalAPI.Data
{
    public class ClinicaDbContext : DbContext
    {

        public ClinicaDbContext(DbContextOptions<ClinicaDbContext> options) : base(options) { }

        public DbSet<Doctor> Doctores { get; set; }
        public DbSet<Admin> Adminstradores { get; set; } //Solo se utiliza para el admin
        public DbSet<Paciente> Pacientes { get; set; }


    }
}
