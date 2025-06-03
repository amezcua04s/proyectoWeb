using clinicaApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

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
        public string CedulaProfesional { get; set; }
        public DateTime Nacimiento { get; set; }

        // Especialidades
        public List<int> EspecialidadesSeleccionadas { get; set; } = new();
        public List<SelectListItem> EspecialidadesDisponibles { get; set; } = new();

        // Disponibilidad Horaria
        public Dictionary<string, string> DisponibilidadesPorDia { get; set; } = new();

        // Foto
        public string? FotoActual { get; set; }
        public IFormFile? NuevaFoto { get; set; }
    }
}
