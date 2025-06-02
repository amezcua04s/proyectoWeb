using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace clinicaApp.Models
{

    /*
     Registradas por el admin
        Permitir al usuario buscar doctor por especialidad (Si tiene al menos más de un doctor asociado a)
        1 medico tiene una disponibilidad (1 : M)
    Disponibilidad es un catalogo definido por el admin
        1 disponibilidad tiene muchos medicos (1 : M)
     */
    public class Disponibilidad
    {
        public int Id { get; set; }

        [Required]
        public int MedicoId { get; set; }
        public Medico Medico { get; set; }

        [Required]
        public Dias DiaDeLaSemana { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan HoraInicio { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan HoraFin { get; set; }

        public bool EstaOcupado { get; set; } = false;

        public string? TimeZoneId { get; set; }
    }

}
