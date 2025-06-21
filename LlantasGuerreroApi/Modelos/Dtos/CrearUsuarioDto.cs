using System.ComponentModel.DataAnnotations;

namespace LlantasGuerreroApi.Modelos.Dtos
{
    public class CrearUsuarioDto
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string NombreCompleto { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Contraseña { get; set; }
        public string CorreoElectronico { get; set; }

    }
}
