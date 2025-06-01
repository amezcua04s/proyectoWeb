using clinicaApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace clinicaApp.ViewModels
{
    public class MedicoViewModel
    {
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string Correo { get; set; }
        public string Contrasenia { get; set; }
        public string Telefono { get; set; }
        public Sexo Sexo { get; set; }
        public DateTime Nacimiento { get; set; }
        public string CedulaProfesional { get; set; }
        public List<string>? Especialidades { get; set; }
        public List<SelectListItem> EspecialidadesDis { get; set; } = new();


        //Disponibilidad del medico, cada día tendra una lista de horas "disponibles"
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

        public IFormFile? Foto { get; set; }
    }
}
