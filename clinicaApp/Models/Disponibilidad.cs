using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace clinicaApp.Models
{

    /*
     Registradas por el admin
        Permitir al usuario buscar doctor por especialidad (Si tiene al menos más de un doctor asociado a)
        1 medico tiene una disponibilidad (1 : 1)
    Disponibilidad es un catalogo definido por el admin
        1 disponibilidad tiene muchos medicos (1 : M)
        1 disponibilidad tiene un dia de la semana (1 : 1)
        1 disponibilidad tiene una hora de inicio y fin (1 : 1)
        1 disponibilidad tiene una zona horaria (1 : 1)
     */
    public class Disponibilidad
    {
        public int Id { get; set; }

        // Clave foránea para el médico al que pertenece esta disponibilidad
        public int MedicoId { get; set; }
        public Medico Medico { get; set; }

        // Día de la semana (ENUM @Enumeraciones)
        [Required]
        public Dias DiaDeLaSemana { get; set; }

        // Hora de inicio y fin de la disponibilidad
        [Required]
        [DataType(DataType.Time)]
        public TimeSpan HoraInicio { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan HoraFin { get; set; }

        // ID de la zona horaria para esta disponibilidad
        // Este ID se usará para obtener un objeto TimeZoneInfo
        [Required]
        public string TimeZoneId { get; set; }

    }
}
