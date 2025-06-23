namespace LlantasGuerreroApi.Modelos.Dtos
{
    public class MovimientosVentaFiltro
    {
        public int IdArticulo { get; set; } = 0;
        public DateTime Fecha { get; set; } = DateTime.MinValue;
        public string NombreUsuario { get; set; } = string.Empty;
    }
}
