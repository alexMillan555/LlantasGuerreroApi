using System.ComponentModel.DataAnnotations;

namespace LlantasGuerreroApi.Modelos
{
    public class CatArticulos
    {
        [Key]
        public int IdArticulo { get; set; }
        [Required]
        public string Clave { get; set; }
        [Required]
        public string Nombre  { get; set; }
        [Required]
        public double Importe { get; set; }
        [Required]
        public int Cantidad { get; set;}
        [Required]
        public int IdEstatus { get; set; } = 3;
        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        [Required]
        public bool Activo { get; set; } = true;

    }
}
