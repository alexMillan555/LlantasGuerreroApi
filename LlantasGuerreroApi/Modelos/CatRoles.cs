using System.ComponentModel.DataAnnotations;

namespace LlantasGuerreroApi.Modelos
{
    public class CatRoles
    {
        [Key]
        public int IdRol { get; set; }
        public string NombreRol { get; set; }
        public int Activo { get; set; }
    }
}
