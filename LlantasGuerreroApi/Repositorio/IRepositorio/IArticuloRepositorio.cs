using LlantasGuerreroApi.Modelos;

namespace LlantasGuerreroApi.Repositorio.IRepositorio
{
    public interface IArticuloRepositorio
    {
        ICollection<CatArticulos> ObtenerArticulos();
        CatArticulos ObtenerArticulo(int idArticulo);
        CatArticulos ObtenerArticulo(string nombreArticulo);
        IEnumerable<CatArticulos> BuscarClaveArticulo(string Clave);
        bool CrearArticulo(CatArticulos articulo);
        bool ActualizarArticulo(CatArticulos articulo);
        bool EliminarArticulo(CatArticulos articulo);
        bool ExisteArticulo(string nombreArticulo);
        bool Guardar();
    }
}
