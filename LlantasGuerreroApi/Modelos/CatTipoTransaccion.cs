using System.ComponentModel.DataAnnotations;

namespace LlantasGuerreroApi.Modelos
{
    public class CatTipoTransaccion
    {
        [Key]
        public int IdTipoTransaccion { get; set; }
        public string NombreTipoTransaccion { get; set; }
        public bool Activo { get; set; }
    }
}
