using LlantasGuerreroApi.Modelos;
using LlantasGuerreroApi.Modelos.Dtos;

namespace LlantasGuerreroApi.Repositorio.IRepositorio
{
    public interface IArticuloRepositorio
    {
        ICollection<CatArticulosDto> ObtenerArticulos();
        CatArticulosDto ObtenerArticulo(int idArticulo);
        CatArticulos ObtenerArticulo(string nombreArticulo);
        IEnumerable<CatArticulosDto> BuscarClaveArticulo(string Clave);
        bool CrearArticulo(CatArticulos articulo, string nombreUsuario, CrearArticuloDto crearArticuloDto);
        bool ActualizarArticulo(CatArticulos articulo, ActualizarArticulosDto actualizarArticulosDto);
        bool BajaArticulo(CatArticulos articulo, BajaArticuloDto bajaArticuloDto);
        bool EliminarArticulo(CatArticulos articulo);
        bool ExisteArticulo(string nombreArticulo);
        bool Guardar();
        bool CrearPropiedadArticulo(CrearPropiedadArticuloDto crearPropiedadArticuloDto);
    }
}
