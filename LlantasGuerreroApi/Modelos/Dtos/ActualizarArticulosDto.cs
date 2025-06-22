namespace LlantasGuerreroApi.Modelos.Dtos
{
    public class ActualizarArticulosDto
    {
        public int IdArticulo { get; set; }
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        //public string PropiedadValor { get; set; }
    }
}
