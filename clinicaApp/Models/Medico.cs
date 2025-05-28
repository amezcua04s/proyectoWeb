using clinicaApp.Models;
using System.ComponentModel.DataAnnotations;

public class Medico
{
    public int Id { get; set; }
    [Required]
    public string UserId { get; set; }
    public ClinicaUser User { get; set; } 

    public Sexo Sexo { get; set; }
    public DateTime Nacimiento { get; set; }

    public String Especialidad { get; set; }
    public string CedulaProfesional { get; set; }
    public string Notas { get; set; }
    public string Telefono { get; set; }
    public string Foto { get; set; }

    // Ahora el médico tiene una colección de DisponibilidadMedico
    public ICollection<Disponibilidad> Disponibilidades { get; set; }

    public ICollection<Cita> Citas { get; set; }
    public ICollection<Expediente> Expedientes { get; set; }

    public Medico()
    {
        Disponibilidades = new List<Disponibilidad>();
        Citas = new List<Cita>();
        Expedientes = new List<Expediente>();
    }
}
