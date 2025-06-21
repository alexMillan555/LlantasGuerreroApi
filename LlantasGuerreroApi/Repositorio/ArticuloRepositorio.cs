using AutoMapper;
using LlantasGuerreroApi.Datos;
using LlantasGuerreroApi.Modelos;
using LlantasGuerreroApi.Repositorio.IRepositorio;

namespace LlantasGuerreroApi.Repositorio
{
    public class ArticuloRepositorio : IArticuloRepositorio
    {
        private readonly ContextoAplicacionBD _bd;
        private readonly IMapper _mapper;

        public ArticuloRepositorio(ContextoAplicacionBD bd, IMapper mapper)
        {
            _bd = bd;
            _mapper = mapper;
        }

        public bool ActualizarArticulo(CatArticulos articulo)
        {
            var articuloExistente = _bd.CatArticulos.Find(articulo.IdArticulo);

            if(articuloExistente != null)
                _bd.Entry(articuloExistente).CurrentValues.SetValues(articulo);
            else
                _bd.CatArticulos.Update(articulo);

            return Guardar();
        }

        public IEnumerable<CatArticulos> BuscarClaveArticulo(string Clave)
        {
            IQueryable<CatArticulos> query = _bd.CatArticulos;
            if(!string.IsNullOrEmpty(Clave))            
                query = query.Where(a => a.Clave.ToLower().Contains(Clave.ToLower()));

            return query.ToList();
        }

        public bool CrearArticulo(CatArticulos articulo, int idUsuario)
        {
            _bd.CatArticulos.Add(articulo);

            Movimientos movimiento = new Movimientos()
            {
                Cantidad = articulo.Cantidad,
                IdTipoTransaccion = 1, // Asumiendo que 1 es para ingreso
                IdEstatus = 7, // Asumiendo que 7 es para finalizado
                Observaciones = "Ingreso inicial del artículo",
                IdUsuario = idUsuario
            };
            _bd.Movimientos.Add(movimiento);

            MovimientosDetalle movimientosDetalle = new MovimientosDetalle()
            {
                Cantidad = articulo.Cantidad,
                IdArticulo = articulo.IdArticulo,
                IdMovimiento = movimiento.IdMovimiento,
                IdEstatus = 7 // Asumiendo que 7 es para finalizado
            };
            _bd.MovimientosDetalle.Add(movimientosDetalle);

            return Guardar();
        }

        public bool EliminarArticulo(CatArticulos articulo)
        {
            _bd.CatArticulos.Remove(articulo);
            return Guardar();
        }

        public bool ExisteArticulo(string nombreArticulo)
        {
            bool valor = _bd.CatArticulos.Any(a => a.Nombre.ToUpper() == nombreArticulo.ToUpper());
            return valor;
        }

        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true : false;
        }

        public CatArticulos ObtenerArticulo(int idArticulo)
        {
            return _bd.CatArticulos.FirstOrDefault(a => a.IdArticulo == idArticulo);
        }

        public CatArticulos ObtenerArticulo(string nombreArticulo)
        {
            return _bd.CatArticulos.FirstOrDefault(a => a.Nombre.ToUpper() == nombreArticulo.ToUpper());
        }

        public ICollection<CatArticulos> ObtenerArticulos()
        {
            return _bd.CatArticulos.OrderBy(u => u.Nombre).ToList();
        }
    }   
}
