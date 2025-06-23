using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LlantasGuerreroApi.Modelos.Dtos
{
    public class ArticuloEntradaDto
    {
        public int IdArticulo { get; set; }
        public float ArticuloEntradaImporte { get; set; }
        public int ArticuloEntradaCantidad { get; set; }
        public string ArticuloEntradaObservaciones { get; set; }
    }
}
