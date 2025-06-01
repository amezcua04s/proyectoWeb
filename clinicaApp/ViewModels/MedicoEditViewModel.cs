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
        public Dictionary<string, List<string>> DisponibilidadesPorDia { get; set; } = new()
        {
            { "Lunes", new List<string>() },
            { "Martes", new List<string>() },
            { "Miércoles", new List<string>() },
            { "Jueves", new List<string>() },
            { "Viernes", new List<string>() },
            { "Sábado", new List<string>() },
            { "Domingo", new List<string>() },
        };

        public List<string> TodasLasHoras { get; set; } = new()
        {
            "07:00", "08:00", "09:00", "10:00", "11:00",
            "12:00", "13:00", "14:00", "15:00", "16:00",
            "17:00", "18:00", "19:00", "20:00"
        };

        // Foto
        public string? FotoActual { get; set; }
        public IFormFile? NuevaFoto { get; set; }
    }
}
