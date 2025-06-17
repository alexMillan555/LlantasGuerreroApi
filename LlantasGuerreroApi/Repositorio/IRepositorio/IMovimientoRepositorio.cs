using LlantasGuerreroApi.Modelos;

namespace LlantasGuerreroApi.Repositorio.IRepositorio
{
    public interface IMovimientoRepositorio
    {
        ICollection<Movimientos> ObtenerMovimientos();
        Movimientos ObtenerMovimiento(int idMovimiento);
        bool CrearMovimiento(Movimientos movimiento);
        bool ActualizarMovimiento(Movimientos movimiento);
        bool EliminarMovimiento(Movimientos movimiento);
        bool ExisteMovimiento(string IdMovimiento);
        bool Guardar();
    }
}
