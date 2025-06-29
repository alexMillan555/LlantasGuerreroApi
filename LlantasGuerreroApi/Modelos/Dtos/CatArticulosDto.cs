﻿using System.ComponentModel.DataAnnotations;

namespace LlantasGuerreroApi.Modelos.Dtos
{
    public class CatArticulosDto
    {
        public int IdArticulo { get; set; }
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public double Importe { get; set; }
        public int Cantidad { get; set; }
        public int IdEstatus { get; set; }
        public DateTime FechaRegistro { get; set; } 
        public bool Activo { get; set; }
        public string PropiedadNombre { get; set; }
        public string PropiedadValor { get; set; }
    }
}
