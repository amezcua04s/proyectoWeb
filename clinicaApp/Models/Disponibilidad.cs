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

        [Required]
        public int MedicoId { get; set; }
        public Medico Medico { get; set; }

        // Día de la semana
        [Required]
        public Dias DiaDeLaSemana { get; set; }

        // Hora específica de la disponibilidad (ej. 7:00, 8:00)
        [Required]
        [DataType(DataType.Time)]
        public TimeSpan Hora { get; set; }

        // Si ese horario está ocupado o no
        public bool EstaOcupado { get; set; } = false;

        // Zona horaria si la necesitas (opcional)
        public string? TimeZoneId { get; set; }
    }
}
