namespace clinicaApp.Models
{
    public enum Sexo { 
        MASCULINO = 0,
        FEMENINO = 1,
        OTRO = 2

    }

    public enum Dias {
        DOMINGO = 0,
        LUNES = 1,
        MARTES = 2,
        MIERCOLES = 3,
        JUEVES = 4,
        VIERNES = 5,
        SABADO = 6
    }

    public enum EstadoCita {

        Pendiente = 0,
        Completada = 1,
        Cancelada = 2,
        NoAsistio = 3

    }

}
