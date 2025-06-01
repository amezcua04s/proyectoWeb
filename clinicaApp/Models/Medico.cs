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
        public List<string>? Especialidades { get; set; }
        public List<string>? Disponibilidades { get; set; } //lista de disponibilidades del médico Lunes 7-8, Lunes 8-9 , etc.


        //public ICollection<MedicoEspecialidad>? Especialidades { get; set; } // lista de especialidades del médico (1 : M)
        //public ICollection<Cita>? Citas { get; set; } // lista de las citas del médico Lunes 7-8, Lunes 8-9, etc. (1 : M)
        //public ICollection<Expediente>? Expedientes { get; set; } // lista de expedientes del médico (1 : M)

        //Para las relaciones
        //relacion con las especialidades
        public ICollection<MedicoEspecialidad>? MedicoEspecialidades { get; set; }


    }

}
