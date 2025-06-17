using System.ComponentModel.DataAnnotations;

namespace LlantasGuerreroApi.Modelos.Dtos
{
    public class LoginUsuarioDto
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Contraseña { get; set; }
    }
}
