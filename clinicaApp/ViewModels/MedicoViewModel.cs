using clinicaApp.Models;

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
        public string Especialidad { get; set; }
        public string CedulaProfesional { get; set; }
        public IFormFile? Foto { get; set; }
    }
}
