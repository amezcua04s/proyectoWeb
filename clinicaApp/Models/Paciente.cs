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
        public List<String> Alergias { get; set; } = new List<String>();
        public List<String> Enfermedades { get; set; } = new List<String>();
        public List<String> Medicamentos { get; set; } = new List<String>();
        public List<String> Antecedentes { get; set; } = new List<String>();
        public List<String> Cirugias { get; set; } = new List<String>();
        public List<String> Tratamientos { get; set; } = new List<String>();

        //Para la relacion con citas
        public ICollection<Cita> Citas { get; set; }

        //Para la relacion con expediente
        public Expediente Expediente { get; set; }

    }
}
