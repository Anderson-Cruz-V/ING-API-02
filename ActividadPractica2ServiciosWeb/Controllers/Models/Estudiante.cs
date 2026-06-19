namespace ActividadPractica2ServiciosWeb.Models
{
    public class Estudiante
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = "";

        public string Apellido { get; set; } = "";

        public string Correo { get; set; } = "";

        public string Carrera { get; set; } = "";

        public int Edad { get; set; }

        public decimal Promedio { get; set; }

        public bool Activo { get; set; }
    }
}