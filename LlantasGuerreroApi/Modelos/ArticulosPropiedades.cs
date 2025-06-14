using System.ComponentModel.DataAnnotations.Schema;

namespace LlantasGuerreroApi.Modelos
{
    public class ArticulosPropiedades
    {
        public int IdArticuloPropiedad { get; set; }        
        public string PropiedadValor { get; set; }
        public int IdArticulo { get; set; }
        [ForeignKey("IdArticulo")]
        public CatArticulos Articulo { get; set; }
        public int IdPropiedad { get; set; }
        [ForeignKey("IdPropiedad")]
        public CatPropiedades Propiedad { get; set; }
        public int Activo { get; set; }
    }
}
