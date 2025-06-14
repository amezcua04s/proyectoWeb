﻿namespace clinicaApp.Models
{

    /*
     Registradas por el admin, para inflar página principal
     Permitir al usuario buscar doctor por especialidad (Si tiene al menos más de un doctor asociado a)
     

    1 especialidad tiene muchos doctores (1 : M) 
    1 doctor tiene muchas especialidades (1 : M)
    Muchos a Muchos (TRANSITIVA)

    Especialidad es un catalogo
*/     
    public class Especialidad
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; } = "";

        //Para la relacion con mediocs
        public ICollection<MedicoEspecialidad>? MedicoEspecialidades { get; set; }


    }
}
