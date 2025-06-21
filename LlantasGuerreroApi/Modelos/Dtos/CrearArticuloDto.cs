using System.ComponentModel.DataAnnotations;

namespace LlantasGuerreroApi.Modelos.Dtos
{
    public class CrearArticuloDto
    {
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public double Importe { get; set; }
        public int Cantidad { get; set; }
    }
}
