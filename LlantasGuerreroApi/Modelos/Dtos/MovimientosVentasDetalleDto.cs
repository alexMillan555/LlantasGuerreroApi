
namespace LlantasGuerreroApi.Modelos.Dtos
{
    public class MovimientosVentasDetalleDto
    {
        public string Articulo { get; set; }
        public int Cantidad { get; set; }
        public float PrecioUnitario { get; set; }
        public float Total { get; set; }
        public DateTime FechaVenta { get; set; }
        public string Observaciones { get; set; }
        public string NombreCliente { get; set; }
    }
}
