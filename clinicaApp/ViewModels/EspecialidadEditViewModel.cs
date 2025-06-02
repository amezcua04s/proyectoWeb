﻿using System.ComponentModel.DataAnnotations;

namespace clinicaApp.ViewModels
{
    public class EspecialidadEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        public string Descripcion { get; set; } = "";
    }
}
