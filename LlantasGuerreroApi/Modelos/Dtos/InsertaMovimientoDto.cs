namespace LlantasGuerreroApi.Modelos.Dtos
{
    public class InsertaMovimientoDto
    {
        public int Cantidad { get; set; }
        public int IdTipoTransaccion { get; set; } // Entrada o Salida
        public int IdEstatus { get; set; }
        public string Observaciones { get; set; }
        public int IdUsuario { get; set; }
    }
}
