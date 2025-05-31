using System.ComponentModel.DataAnnotations;
using clinicaApp.Models;

namespace clinicaApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Nombre { get; set; }

        public string Paterno { get; set; }

        public string Materno { get; set; }

        [Required, EmailAddress]
        public string Correo { get; set; }
        public string ConfirmCorreo { get; set; }

        [Required, DataType(DataType.Password)]
        public string Contrasenia { get; set; }

        public string Telefono { get; set; }

        [Required]
        public Sexo Sexo { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime Nacimiento { get; set; }

        public List<string> Alergias { get; set; } = new();
        public List<string> Enfermedades { get; set; } = new();
        public List<string> Medicamentos { get; set; } = new();
        public List<string> Antecedentes { get; set; } = new();
        public List<string> Cirugias { get; set; } = new();
        public List<string> Tratamientos { get; set; } = new();
    }
}
