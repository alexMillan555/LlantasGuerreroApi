using System.ComponentModel.DataAnnotations;

namespace LlantasGuerreroApi.Modelos
{
    public class Usuarios
    {
        [Key]
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string Contraseña { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
