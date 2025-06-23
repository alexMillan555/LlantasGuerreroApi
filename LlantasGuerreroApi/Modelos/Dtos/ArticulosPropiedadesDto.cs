using System.ComponentModel.DataAnnotations.Schema;

namespace LlantasGuerreroApi.Modelos.Dtos
{
    public class ArticulosPropiedadesDto
    {
        public string Articulo { get; set; }
        public string PropiedadNombre { get; set; }
        public string PropiedadValor { get; set; }
        public int Activo { get; set; }
    }
}
