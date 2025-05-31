using Microsoft.AspNetCore.Identity;

namespace clinicaApp.Models
{
    public class ClinicaUser : IdentityUser
    {

        public string Nombre { get; set; }
        public string Materno { get; set; }
        public string? Paterno { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Contrasenia { get; set; }
        public Sexo Sexo { get; set; } = Sexo.OTRO;
        public bool Activo { get; set; } = true;
        public DateTime Registro { get; set; } = DateTime.Now;
        public DateTime? Modificacion { get; set; }

    }
}
