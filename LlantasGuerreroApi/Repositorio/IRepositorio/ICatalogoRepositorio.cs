using LlantasGuerreroApi.Modelos;
using LlantasGuerreroApi.Modelos.Dtos;

namespace LlantasGuerreroApi.Repositorio.IRepositorio
{
    public interface ICatalogoRepositorio
    {
        ICollection<CatRoles> ObtenerRoles();
    }
}
