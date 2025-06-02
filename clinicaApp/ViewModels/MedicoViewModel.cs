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

        //Lista de los id's de especialidades que estarán asociadas al doctor
        public List<int>? EspecialidadesSeleccionadas { get; set; }

        //Lista de las especialidades que existen registradas
        public List<SelectListItem> EspecialidadesDisponibles { get; set; } = new();

        //Disponibilidad del medico, cada día tendra una lista de horas "disponibles"
        public Dictionary<string, string> DisponibilidadesPorDia { get; set; } = new()
        {
            { "Lunes", "" },
            { "Martes", "" },
            { "Miércoles", "" },
            { "Jueves", "" },
            { "Viernes", "" },
            { "Sábado", "" },
            { "Domingo", "" }
        };


        public IFormFile? Foto { get; set; }

    }
}
