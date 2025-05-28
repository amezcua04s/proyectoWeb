namespace clinicaApp.Models
{

    /*
     Generada por el paciente si es que se puede (disponibilidad del doctor)
        1 paciente tiene muchas citas (1 : M)
        1 doctor tiene muchas citas (1 : M)

    CITA ES LA TABLA TRANSITIVA
        Una cita tiene un paciente (1 : 1)
        Una cita tiene un doctor (1 : 1)

    
     */
    public class Cita
    {
        public int Id { get; set; }
        public string PacienteId { get; set; }

        public int MedicoId { get; set; }
        public Medico Medico { get; set; }
        public DateTime FechaHora { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public string Motivo { get; set; }
        public EstadoCita Estado { get; set; } = EstadoCita.Pendiente;
        public string Notas { get; set; }
    }
}
