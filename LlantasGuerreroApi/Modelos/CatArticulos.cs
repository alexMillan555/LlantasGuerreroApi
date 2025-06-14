﻿using System.ComponentModel.DataAnnotations;

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
        public DateTime FechaRegistro { get; set; }
        [Required]
        public bool Activo { get; set; }

    }
}
