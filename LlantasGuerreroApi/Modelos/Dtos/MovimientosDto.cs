using System.ComponentModel.DataAnnotations.Schema;

namespace LlantasGuerreroApi.Modelos.Dtos
{
    public class MovimientosDto
    {
        public int IdMovimiento { get; set; }
        public int Cantidad { get; set; }
        public int IdTipoTransaccion { get; set; } // Entrada o Salida
        public int IdEstatus { get; set; }
        public DateTime FechaMovimiento { get; set; } = DateTime.Now;
        public string Observaciones { get; set; }
        public int IdUsuario { get; set; }
    }
}
