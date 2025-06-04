using System.ComponentModel.DataAnnotations;

namespace clinicaApp.Models {
    public class Medico
    {
        public int Id { get; set; }
        [Required]
        public string? UserId { get; set; }
        public ClinicaUser? User { get; set; }
        public DateTime Nacimiento { get; set; }
        public string CedulaProfesional { get; set; }
        public string? Foto { get; set; }
        public List<string>? Especialidades { get; set; } //Convierte la lista de las especialidades en texto
        public List<Disponibilidad> Disponibilidades { get; set; } = new();
        public Boolean primerInicio { get; set; } = true;

        //relacion con las especialidades
        public ICollection<MedicoEspecialidad>? MedicoEspecialidades { get; set; }

        //relacion con las citas
        public ICollection<Cita> Citas { get; set; } = new List<Cita>();

    }

}
