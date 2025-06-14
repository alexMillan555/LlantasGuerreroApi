using System.ComponentModel.DataAnnotations;

namespace LlantasGuerreroApi.Modelos
{
    public class CatEstatus
    {
        [Key]
        public int IdEstatus { get; set; }
        public string NombreEstatus { get; set; }
        public bool Activo { get; set; }
    }
}
