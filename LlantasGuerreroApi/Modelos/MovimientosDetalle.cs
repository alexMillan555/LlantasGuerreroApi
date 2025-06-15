using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LlantasGuerreroApi.Modelos
{
    public class MovimientosDetalle
    {
        [Key]
        public int IdMovimientosDetalle { get; set; }
        public int IdMovimiento { get; set; }
        [ForeignKey("IdMovimiento")]
        public Movimientos Movimientos { get; set; }
        public int IdArticulo { get; set; }
        [ForeignKey("IdArticulo")]
        public CatArticulos CatArticulo { get; set; }
        public int Cantidad { get; set; }
        public int CantidadPendiente { get; set; }
        public int IdEstatus { get; set; }
        [ForeignKey("IdEstatus")]
        public CatEstatus CatEstatus { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
