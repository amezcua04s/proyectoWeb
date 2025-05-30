namespace clinicaApp.Models
{
    /*
     Un expediente tiene un pacienta (1 : 1)
     Un paciente tiene un expediente (1 : 1)
     */

    public class Expediente
    {
        public int Id { get; set; }
        public string PacienteId { get; set; }

        public int MedicoId { get; set; }
        public Medico Medico { get; set; }
        public DateTime FechaModificacion { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public string Notas { get; set; }
        public string Padecimiento { get; set; }
        public string Tratamiento { get; set; }
        public string Diagnostico { get; set; }
        public string Observaciones { get; set; }
        public string Receta { get; set; }
    }
}
