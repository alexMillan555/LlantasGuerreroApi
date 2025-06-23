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
        bool ExisteArticulo(int idArticulo);
        bool Guardar();
        bool ExistePropiedad(string nombrePropiedad);
        bool ExistePropiedad(int idPropiedad);
        bool ExistePropiedadArticulo(int IdArticulo, int IdPropiedad);
        bool CrearPropiedadArticulo(CrearPropiedadArticuloDto crearPropiedadArticuloDto);
        ArticulosPropiedadesDto VerPropiedadesArticulo(int idArticulo);
        bool ActualizarPropiedadArticulo(ActualizarArticuloPropiedadDto actualizarArticuloPropiedadDto);
        bool ActualizarPropiedadArticuloNombre(ActualizarNombrePropiedadDto actualizarNombrePropiedadDto);
        bool EliminarPropiedadArticulo(EliminarPropiedadArticuloDto eliminarPropiedadArticuloDto);
        bool EliminarPropiedad(EliminarPropiedadDto eliminarPropiedadDto);
        bool ArticuloEntrada(ArticulosEntradas articuloEntrada, ArticuloEntradaDto articuloEntradaDto, string nombreUsuario);
    }
}
