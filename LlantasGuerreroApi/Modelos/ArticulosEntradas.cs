using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LlantasGuerreroApi.Modelos
{
    public class ArticulosEntradas
    {
        [Key]
        public int IdArticuloEntrada { get; set; }
        public int IdArticulo { get; set; }
        [ForeignKey("IdArticulo")]
        public CatArticulos CatArticulo { get; set; }
        public float ArticuloEntradaImporte { get; set; }
        public int ArticuloEntradaCantidad { get; set; }
        public int IdMovimiento { get; set; }
        [ForeignKey("IdMovimiento")]
        public Movimientos Movimiento { get; set; }
        public DateTime FechaEntradaRegistro { get; set; } = DateTime.Now;
        public string ArticuloEntradaObservaciones { get; set; }

    }
}
