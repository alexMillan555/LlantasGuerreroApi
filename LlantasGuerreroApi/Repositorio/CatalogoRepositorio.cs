using AutoMapper;
using LlantasGuerreroApi.Datos;
using LlantasGuerreroApi.Modelos;
using LlantasGuerreroApi.Repositorio.IRepositorio;

namespace LlantasGuerreroApi.Repositorio
{
    public class CatalogoRepositorio : ICatalogoRepositorio
    {
        private readonly ContextoAplicacionBD _bd;
        private readonly IMapper _mapper;

        public CatalogoRepositorio(ContextoAplicacionBD bd, IMapper mapper)
        {
            _bd = bd;
            _mapper = mapper;
        }

        public ICollection<CatRoles> ObtenerRoles()
        {
            return _bd.CatRoles.OrderBy(r => r.NombreRol).ToList();
        }
    }
}
