﻿using AutoMapper;
using LlantasGuerreroApi.Modelos;
using LlantasGuerreroApi.Modelos.Dtos;

namespace LlantasGuerreroApi.LlantasGuerreroMapper
{
    public class LlantasGuerreroMapper : Profile
    {
        public LlantasGuerreroMapper()
        {
            CreateMap<CatArticulos, AltaArticuloDto>().ReverseMap();
            CreateMap<CatArticulos, ActualizarArticulosDto>().ReverseMap();
            CreateMap<Usuarios, UsuarioDto>().ReverseMap();
            CreateMap<CatArticulos, CatArticulosDto>().ReverseMap();
            CreateMap<CatArticulos, CrearArticuloDto>().ReverseMap();
            CreateMap<CatArticulos, BajaArticuloDto>().ReverseMap();
            CreateMap<ArticulosEntradas, ArticuloEntradaDto>().ReverseMap();
            CreateMap<Movimientos, MovimientosDto>().ReverseMap();
            CreateMap<Movimientos, MovimientoVentaDto>().ReverseMap();
            CreateMap<Movimientos, MovimientosVentasDetalleDto>().ReverseMap();
            CreateMap<CatRoles, CatRolesDto>().ReverseMap();
        }
    }
}
