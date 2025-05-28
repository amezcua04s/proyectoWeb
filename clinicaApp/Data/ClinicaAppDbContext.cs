using clinicaApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace clinicaApp.Data
{
    public class ClinicaAppDbContext : IdentityDbContext<ClinicaUser>
    {

        public ClinicaAppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Cita> Citas { get; set; }
        public DbSet<Disponibilidad> Disponibilidades { get; set; }
        public DbSet<Expediente> Expedientes { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }


    }
}
