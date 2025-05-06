namespace hospitalAPI.Models
{
    public class Admin
    {

        public string usuario { get; set; }
        public string email { get; set; }
        public string contraseña { get; set; }
        public bool contraseñaTemporal { get; set; } = true; //Cuando el usuario inicie sesión por primera vez, pedir contraseña nueva

    }
}
