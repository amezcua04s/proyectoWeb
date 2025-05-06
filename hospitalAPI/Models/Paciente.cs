namespace hospitalAPI.Models
{
    public class Paciente
    {
        public int idPaciente { get; set; }
        public string nombre { get; set; }
        public string paterno { get; set; }
        public string materno { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public string contraseña { get; set; }
        public DateTime fecha_nacimiento { get; set; }
        public Sexo sexo { get; set; }
        public bool estado { get; set; } = true; //activo
        public List<String> alergias { get; set; } = new List<String>();
        public List<String> enfermedades { get; set; } = new List<String>();
        public List<String> medicamentos { get; set; } = new List<String>();
        public List<String> antecedentes { get; set; } = new List<String>();
        public List<String> cirugias { get; set; } = new List<String>();
        public List<String> tratamientos { get; set; } = new List<String>();

    }
}
