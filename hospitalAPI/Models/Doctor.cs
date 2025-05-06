namespace hospitalAPI.Models
{
    public enum Sexo { Masculino, Femenino }
    public class Doctor
    {
        public int idDoctor { get; set; }
        public string nombre { get; set; }
        public string paterno { get; set; }
        public string materno { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public string contraseña { get; set; }
        public DateTime fecha_nacimiento { get; set; }
        public Sexo sexo { get; set; }
        public bool estado { get; set; } = true; //activo

        public List<String> especialidad { get; set; } = new List<String>();
        public List<String> horario { get; set; } = new List<String>();    

    }
}
