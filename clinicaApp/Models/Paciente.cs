using System.ComponentModel.DataAnnotations;

namespace clinicaApp.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public ClinicaUser User { get; set; }
        public DateTime Nacimiento { get; set; }
        public List<String> alergias { get; set; } = new List<String>();
        public List<String> enfermedades { get; set; } = new List<String>();
        public List<String> medicamentos { get; set; } = new List<String>();
        public List<String> antecedentes { get; set; } = new List<String>();
        public List<String> cirugias { get; set; } = new List<String>();
        public List<String> tratamientos { get; set; } = new List<String>();


    }
}
