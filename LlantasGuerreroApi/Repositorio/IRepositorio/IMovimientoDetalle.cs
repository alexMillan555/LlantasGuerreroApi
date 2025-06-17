using LlantasGuerreroApi.Modelos;

namespace LlantasGuerreroApi.Repositorio.IRepositorio
{
    public interface IMovimientoDetalle
    {
        ICollection<MovimientosDetalle> ObtenerMovimientosDetalles();
        MovimientosDetalle ObtenerMovimientoDetalle(int idMovimientoDetalle);
        IEnumerable<MovimientosDetalle> ObtenerMovimientoDetallesPorMovimiento(int idMovimiento);
        IEnumerable<MovimientosDetalle> ObtenerMovimientoDetallesPorArticulo(int idArticulo);
        bool CrearMovimientoDetalle(MovimientosDetalle movimientoDetalle);
        bool ActualizarMovimientoDetalle(MovimientosDetalle movimientoDetalle);
        bool EliminarMovimientoDetalle(MovimientosDetalle movimientoDetalle);
        bool ExisteMovimientoDetalle(string IdMovimientoDetalle);
        bool Guardar();
    }
}
