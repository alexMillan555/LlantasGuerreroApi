using AutoMapper;
using LlantasGuerreroApi.Modelos;
using LlantasGuerreroApi.Modelos.Dtos;

namespace LlantasGuerreroApi.LlantasGuerreroMapper
{
    public class LlantasGuerreroMapper : Profile
    {
        public LlantasGuerreroMapper()
        {
            CreateMap<CatArticulos, CatArticulosDto>().ReverseMap();
        }
    }
}
