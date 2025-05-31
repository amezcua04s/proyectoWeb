using clinicaApp.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace clinicaApp.ViewModels
{
    public class MedicoEditViewModel
    {
        public int Id { get; set; }

        // Datos del usuario
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public Sexo Sexo { get; set; }

        // Datos del médico
        public string Especialidad { get; set; }
        public string CedulaProfesional { get; set; }
        public DateTime Nacimiento { get; set; }

        public string? FotoActual { get; set; }
        public IFormFile? NuevaFoto { get; set; }
    }
}
