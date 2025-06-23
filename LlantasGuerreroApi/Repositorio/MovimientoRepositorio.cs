using AutoMapper;
using LlantasGuerreroApi.Datos;
using LlantasGuerreroApi.Modelos;
using LlantasGuerreroApi.Modelos.Dtos;
using LlantasGuerreroApi.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LlantasGuerreroApi.Repositorio
{
    public class MovimientoRepositorio : IMovimientoRepositorio
    {
        private readonly ContextoAplicacionBD _bd;
        private readonly IMapper _mapper;

        public MovimientoRepositorio(ContextoAplicacionBD bd, IMapper mapper)
        {
            _bd = bd;
            _mapper = mapper;
        }

        public bool ActualizarMovimiento(Movimientos movimiento)
        {
            throw new NotImplementedException();
        }

        public bool CrearMovimiento(Movimientos movimiento)
        {
            throw new NotImplementedException();
        }

        public bool EliminarMovimiento(Movimientos movimiento)
        {
            throw new NotImplementedException();
        }

        public bool ExisteMovimiento(int IdMovimiento)
        {
            var valor = _bd.Movimientos.Any(m => m.IdMovimiento == IdMovimiento);
            return valor;
        }

        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true : false;
        }

        public bool MovimientoVenta(MovimientoVentaDto movimientoVentaDto, string nombreUsuario)
        {
            var idUsuario = _bd.Usuarios.FirstOrDefault(u => u.NombreUsuario.ToUpper() == nombreUsuario.ToUpper()).IdUsuario;

            var articulos = _bd.CatArticulos.Find(movimientoVentaDto.IdArticulo);

            if(articulos == null)            
                return false; // Artículo no encontrado
            else if((articulos.Cantidad-movimientoVentaDto.Cantidad)<= 0)
                return false; // No hay suficiente stock para la venta
            else
            {
                var Movimiento = new Movimientos
                {
                    Cantidad = movimientoVentaDto.Cantidad,
                    FechaMovimiento = DateTime.Now,
                    IdTipoTransaccion = 6, //PARA VENTA ESTATUS 6
                    IdEstatus = 7, //PARA ESTATUS MOVIMIENTO FINALIZADO
                    Observaciones = movimientoVentaDto.Observaciones,
                    IdUsuario = idUsuario
                };
                _bd.Movimientos.Add(Movimiento);
                Guardar();

                var movimientosDetalle = new MovimientosDetalle
                {
                    IdMovimiento = Movimiento.IdMovimiento,
                    IdArticulo = movimientoVentaDto.IdArticulo,
                    Cantidad = movimientoVentaDto.Cantidad,
                    IdEstatus = 7
                };
                _bd.MovimientosDetalle.Add(movimientosDetalle);
                Guardar();

                // Actualizar el stock del artículo
                articulos.Cantidad -= movimientoVentaDto.Cantidad;
                _bd.Entry(articulos).State = EntityState.Modified;
                _bd.Entry(articulos).Property(a => a.Cantidad).IsModified = true;
            }

            return Guardar();
        }

        public Movimientos ObtenerMovimiento(int idMovimiento)
        {
            return _bd.Movimientos.FirstOrDefault(m => m.IdMovimiento == idMovimiento);
        }

        public ICollection<Movimientos> ObtenerMovimientos()
        {
            return _bd.Movimientos.OrderBy(m => m.FechaMovimiento).ToList();
        }

        public ICollection<MovimientosVentasDetalleDto> ObtenerVentas(string nombreUsuario, int idArticulo, DateTime fecha)
        {
            List<MovimientosVentasDetalleDto> movimientosVenta = new List<MovimientosVentasDetalleDto>();
            if (nombreUsuario.Trim().IsNullOrEmpty() && idArticulo <= 0 && fecha == DateTime.MinValue)
            {
                movimientosVenta = _bd.Movimientos         
                                .Where(m => m.IdTipoTransaccion == 6)
                                .Join(
                                    _bd.MovimientosDetalle,
                                    movimiento => movimiento.IdMovimiento,
                                    detalle => detalle.IdMovimiento,
                                    (movimiento, detalle) => new { Movimiento = movimiento, Detalle = detalle }
                                )
                                .Join(
                                    _bd.CatArticulos,
                                    combined => combined.Detalle.IdArticulo,
                                    articulo => articulo.IdArticulo,
                                    (combined, articulo) => new MovimientosVentasDetalleDto
                                    {

                                        Articulo = articulo.Nombre,
                                        Cantidad = combined.Detalle.Cantidad,
                                        PrecioUnitario = (float)articulo.Importe,
                                        Total = (combined.Detalle.Cantidad * (float)articulo.Importe),
                                        FechaVenta = combined.Detalle.FechaRegistro,
                                        Observaciones = combined.Movimiento.Observaciones,
                                        NombreCliente = _bd.Usuarios
                                            .FirstOrDefault(u => u.IdUsuario == combined.Movimiento.IdUsuario).NombreCompleto
                                    }
                                )
                                .ToList();

            }
            else if(!nombreUsuario.Trim().IsNullOrEmpty())
            {
                var IdUsuario = _bd.Usuarios.FirstOrDefault(u => u.NombreUsuario.ToUpper() == nombreUsuario.ToUpper()).IdUsuario;

                if(IdUsuario> 0)
                {
                    var usuario = _bd.Usuarios.FirstOrDefault(u => u.IdUsuario == IdUsuario);

                    movimientosVenta = _bd.Movimientos
                                .Where(m => m.IdUsuario == IdUsuario)
                                .Join(
                                    _bd.MovimientosDetalle,
                                    movimiento => movimiento.IdMovimiento,
                                    detalle => detalle.IdMovimiento,
                                    (movimiento, detalle) => new { Movimiento = movimiento, Detalle = detalle }
                                )
                                .Where(m => m.Movimiento.IdTipoTransaccion == 6)
                                .Join(
                                    _bd.CatArticulos,
                                    combined => combined.Detalle.IdArticulo,
                                    articulo => articulo.IdArticulo,
                                    (combined, articulo) => new MovimientosVentasDetalleDto
                                    {

                                        Articulo = articulo.Nombre,
                                        Cantidad = combined.Detalle.Cantidad,
                                        PrecioUnitario = (float)articulo.Importe,
                                        Total = (combined.Detalle.Cantidad*(float)articulo.Importe),
                                        FechaVenta = combined.Detalle.FechaRegistro,
                                        Observaciones = combined.Movimiento.Observaciones,
                                        NombreCliente = usuario.NombreCompleto
                                    }
                                )
                                .ToList();
                }
            }
            else if(idArticulo > 0)
            {
                
                movimientosVenta = _bd.MovimientosDetalle.Where(m => m.IdArticulo == idArticulo)
                    .Join(
                            _bd.Movimientos,
                            detalle => detalle.IdMovimiento,
                            movimiento => movimiento.IdMovimiento,
                            (detalle, movimiento) => new { Detalle = detalle, Movimiento = movimiento}
                         )
                    .Where(m => m.Movimiento.IdTipoTransaccion == 6)
                    .Join(
                                    _bd.CatArticulos,
                                    combined => combined.Detalle.IdArticulo,
                                    articulo => articulo.IdArticulo,
                                    (combined, articulo) => new MovimientosVentasDetalleDto
                                    {

                                        Articulo = articulo.Nombre,
                                        Cantidad = combined.Detalle.Cantidad,
                                        PrecioUnitario = (float)articulo.Importe,
                                        Total = (combined.Detalle.Cantidad * (float)articulo.Importe),
                                        FechaVenta = combined.Detalle.FechaRegistro,
                                        Observaciones = combined.Movimiento.Observaciones,
                                        NombreCliente = _bd.Usuarios
                                            .FirstOrDefault(u => u.IdUsuario == combined.Movimiento.IdUsuario).NombreCompleto
                                    }
                                )
                                .ToList();

            }
            else if(fecha != DateTime.MinValue)
            {
                movimientosVenta = _bd.MovimientosDetalle.Where(m => m.FechaRegistro >= fecha)
                    .Join(
                            _bd.Movimientos,
                            detalle => detalle.IdMovimiento,
                            movimiento => movimiento.IdMovimiento,
                            (detalle, movimiento) => new { Detalle = detalle, Movimiento = movimiento }
                         )
                    .Where(m => m.Movimiento.IdTipoTransaccion == 6)
                    .Join(
                                    _bd.CatArticulos,
                                    combined => combined.Detalle.IdArticulo,
                                    articulo => articulo.IdArticulo,
                                    (combined, articulo) => new MovimientosVentasDetalleDto
                                    {

                                        Articulo = articulo.Nombre,
                                        Cantidad = combined.Detalle.Cantidad,
                                        PrecioUnitario = (float)articulo.Importe,
                                        Total = (combined.Detalle.Cantidad * (float)articulo.Importe),
                                        FechaVenta = combined.Detalle.FechaRegistro,
                                        Observaciones = combined.Movimiento.Observaciones,
                                        NombreCliente = _bd.Usuarios
                                            .FirstOrDefault(u => u.IdUsuario == combined.Movimiento.IdUsuario).NombreCompleto
                                    }
                                )
                                .ToList();
            }
            else
            {
                var IdUsuario = _bd.Usuarios.FirstOrDefault(u => u.NombreUsuario.ToUpper() == nombreUsuario.ToUpper()).IdUsuario;
                var usuario = _bd.Usuarios.FirstOrDefault(u => u.IdUsuario == IdUsuario);

                movimientosVenta = _bd.Movimientos
                                .Where(m => m.IdTipoTransaccion == 6 && m.IdUsuario == IdUsuario)
                                .Join(
                                    _bd.MovimientosDetalle,
                                    movimiento => movimiento.IdMovimiento,
                                    detalle => detalle.IdMovimiento,
                                    (movimiento, detalle) => new { Movimiento = movimiento, Detalle = detalle }
                                )
                                .Where(md => md.Detalle.IdArticulo == idArticulo)
                                .Join(
                                    _bd.CatArticulos,
                                    combined => combined.Detalle.IdArticulo,
                                    articulo => articulo.IdArticulo,
                                    (combined, articulo) => new MovimientosVentasDetalleDto
                                    {

                                        Articulo = articulo.Nombre,
                                        Cantidad = combined.Detalle.Cantidad,
                                        PrecioUnitario = (float)articulo.Importe,
                                        Total = (combined.Detalle.Cantidad * (float)articulo.Importe),
                                        FechaVenta = combined.Detalle.FechaRegistro,
                                        Observaciones = combined.Movimiento.Observaciones,
                                        NombreCliente = usuario.NombreCompleto
                                    }
                                )
                                .ToList();
            }

            return movimientosVenta;
        }
    }
}
