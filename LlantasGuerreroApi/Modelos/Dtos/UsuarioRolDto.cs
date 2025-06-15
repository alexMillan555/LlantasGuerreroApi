using System.ComponentModel.DataAnnotations.Schema;

namespace LlantasGuerreroApi.Modelos.Dtos
{
    public class UsuarioRolDto
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public bool Activo { get; set; }
    }
}
