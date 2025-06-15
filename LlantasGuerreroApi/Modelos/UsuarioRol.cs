using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LlantasGuerreroApi.Modelos
{
    public class UsuarioRol
    {
        [Key]
        public int IdUsuarioRol { get; set; }
        public int IdUsuario { get; set; }
        [ForeignKey("IdUsuario")]
        public Usuarios Usuario { get; set; }
        public int IdRol { get; set; }
        [ForeignKey("IdRol")]
        public CatRoles Rol { get; set; }
        public bool Activo { get; set; }
    }
}
