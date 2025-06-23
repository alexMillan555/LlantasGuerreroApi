using LlantasGuerreroApi.Modelos;
using LlantasGuerreroApi.Modelos.Dtos;

namespace LlantasGuerreroApi.Repositorio.IRepositorio
{
    public interface IMovimientoRepositorio
    {
        ICollection<Movimientos> ObtenerMovimientos();
        Movimientos ObtenerMovimiento(int idMovimiento);
        bool CrearMovimiento(Movimientos movimiento);
        bool ActualizarMovimiento(Movimientos movimiento);
        bool EliminarMovimiento(Movimientos movimiento);
        bool ExisteMovimiento(int IdMovimiento);
        bool Guardar();
        bool MovimientoVenta(MovimientoVentaDto movimientoVentaDto, string nombreUsuario);
        ICollection<MovimientosVentasDetalleDto> ObtenerVentas(string nombreUsuario, int idArticulo, DateTime fecha);
    }
}
