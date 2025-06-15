using System.ComponentModel.DataAnnotations.Schema;

namespace LlantasGuerreroApi.Modelos.Dtos
{
    public class ArticulosPropiedadesDto
    {
        public int IdArticuloPropiedad { get; set; }
        public string PropiedadValor { get; set; }
        public int IdArticulo { get; set; }
        public int IdPropiedad { get; set; }
        public int Activo { get; set; }
    }
}
