using System.ComponentModel.DataAnnotations;

namespace LlantasGuerreroApi.Modelos
{
    public class CatPropiedades
    {
        [Key]
        public int IdPropiedad { get; set; }
        public string PropiedadNombre { get; set; }
        public bool Activo { get; set; } = true;
    }
}
