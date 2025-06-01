using clinicaApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;

namespace clinicaApp.Data
{
    public class ClinicaAppDbContext : IdentityDbContext<ClinicaUser>
    {

        public ClinicaAppDbContext(DbContextOptions options) : base(options)
        {
        
        }

        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Especialidad> Especialidades { get; set; }

        public DbSet<Cita> Citas { get; set; }
        public DbSet<Disponibilidad> Disponibilidades { get; set; }
        public DbSet<Expediente> Expedientes { get; set; }

        public DbSet<MedicoEspecialidad> MedicoEspecialidades { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MedicoEspecialidad>()
                .HasKey(me => new { me.MedicoId, me.EspecialidadId });

            modelBuilder.Entity<MedicoEspecialidad>()
                .HasOne(me => me.Medico)
                .WithMany(m => m.MedicoEspecialidades)
                .HasForeignKey(me => me.MedicoId);

            modelBuilder.Entity<MedicoEspecialidad>()
                .HasOne(me => me.Especialidad)
                .WithMany(e => e.MedicoEspecialidades)
                .HasForeignKey(me => me.EspecialidadId);

            // medico - cita
            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Medico)
                .WithMany() // Sin propiedad de navegación inversa
                .HasForeignKey(c => c.MedicoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Paciente)
                .WithMany(p => p.Citas)
                .HasForeignKey(c => c.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
