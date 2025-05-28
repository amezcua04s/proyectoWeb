namespace clinicaApp.Models
{
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
