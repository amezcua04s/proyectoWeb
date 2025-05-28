namespace clinicaApp.Models
{
    /*
     Tabla transitiva Medico - Especialidad
        Permite asociar múltiples especialidades a un médico y viceversa.
        1 médico tiene muchas especialidades (1 : M)
        1 especialidad tiene muchos médicos (1 : M)
        Muchos a Muchos (TRANSITIVA)

        1 MedicoEspecialidad tiene un medico (1 : 1)
        1 MedicoEspecialidad tiene una especialidad (1 : 1) 
     
     */
    public class MedicoEspecialidad
    {

        public int Id { get; set; }
        // Llave foránea para el médico al que pertenece este medicoEspecialidad
        public int MedicoId { get; set; }
        public Medico Medico { get; set; }

        // Llave foránea para la especialidad a la que pertenece este medicoEspecialidad
        public int EspecialidadId { get; set; }
        public Especialidad Especialidad { get; set; }



    }
}
