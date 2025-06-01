using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace clinicaApp.ViewModels
{
    public class EspecialidadViewModel
    {
        [Required(ErrorMessage = "El nombre de la especialidad es obligatorio")]
        public string Nombre { get; set; }
        public string Descripcion { get; set; } = "";


        //No se va a leer, se utiliza para verificaer que la nueva especialidad no exista ya
        [BindNever]
        [ValidateNever]

        public List<string> Especialidades { get; set; }
    }
}
