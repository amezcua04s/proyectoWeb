namespace clinicaApp.Models
{
    /*
     Tabla transitiva Medico - Especialidad
        Permite asociar múltiples especialidades a un médico y viceversa.
        1 médico tiene muchas especialidades (1 : M)
        1 especialidad tiene muchos médicos (1 : M)
        Muchos a Muchos (TRANSITIVA)

        1 MedicoEspecialidad tiene un medico (1 : 1)
        1 MedicoEspecialidad tiene muchas especialidades (1 : M) 
     
     */
    public class MedicoEspecialidad
    {

        public int Id { get; set; }
        // Llave foránea para el médico al que pertenece este medicoEspecialidad
        public int MedicoId { get; set; }
        public Medico Medico { get; set; }

        // llaves foraneas y especialidades que pertenecen a este doctor
        public int EspecialidadId { get; set; }
        public Especialidad Especialidad { get; set; }
        //Acceder solamente al nombre de la especialidad (catalogo)


    }
}
