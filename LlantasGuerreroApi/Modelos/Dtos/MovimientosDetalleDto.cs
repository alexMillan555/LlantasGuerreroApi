using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LlantasGuerreroApi.Modelos.Dtos
{
    public class MovimientosDetalleDto
    {
        public int IdMovimiento { get; set; }
        public int IdArticulo { get; set; }
        public int Cantidad { get; set; }
        public int IdEstatus { get; set; }
    }
}
