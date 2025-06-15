using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LlantasGuerreroApi.Modelos
{
    public class Movimientos
    {
        [Key]
        public int IdMovimiento { get; set; }
        public int Cantidad { get; set; }
        public int IdTipoTransaccion { get; set; } // Entrada o Salida
        [ForeignKey("IdTipoTransaccion")]
        public CatTipoTransaccion TipoTransaccion { get; set; }
        public int IdEstatus { get; set; }
        [ForeignKey("IdEstatus")]
        public CatEstatus Estatus { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public string Observaciones { get; set; }
        public int IdUsuario { get; set; }
        [ForeignKey("IdUsuario")]
        public Usuarios Usuario { get; set; }

    }
}
