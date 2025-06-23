using AutoMapper;
using LlantasGuerreroApi.Datos;
using LlantasGuerreroApi.Modelos;
using LlantasGuerreroApi.Modelos.Dtos;
using LlantasGuerreroApi.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

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

        public bool BajaArticulo(CatArticulos articulo, BajaArticuloDto bajaArticuloDto)
        {
            var articuloExistente = _bd.CatArticulos.Find(articulo.IdArticulo);

            var dtoProperties = typeof(BajaArticuloDto).GetProperties();
            articuloExistente.Activo = false; // Marcar como inactivo
            articuloExistente.IdEstatus = 5; // Asumiendo que 5 es el estatus para baja

            if (articuloExistente != null)
            {
                //Actualizar propiedades específicas sin definir en null o 0 las que no se modifiquen
                foreach (var prop in dtoProperties)
                {
                    if (prop.Name == "IdArticulo") continue; // ¡Excluye el ID!

                    var value = prop.GetValue(bajaArticuloDto);
                    var entityType = _bd.Entry(articuloExistente).Entity.GetType();
                    var entityProp = entityType.GetProperty(prop.Name);

                    if (value != null) // Solo actualiza si el DTO tiene valor
                    {
                        if (entityProp != null) // ¡Solo actualiza si la propiedad existe!
                        {
                            _bd.Entry(articuloExistente).Property(prop.Name).CurrentValue = value;
                            _bd.Entry(articuloExistente).Property(prop.Name).IsModified = true;
                        }
                    }
                }
            }
            else
                return false;       

            return Guardar();
        }

        public bool ActualizarArticulo(CatArticulos articulo, ActualizarArticulosDto actualizarArticulosDto)
        {
            var articuloExistente = _bd.CatArticulos.Find(articulo.IdArticulo);

            var dtoProperties = typeof(ActualizarArticulosDto).GetProperties();

            if (articuloExistente != null)
            {
                //Actualizar propiedades específicas sin definir en null o 0 las que no se modifiquen
                foreach (var prop in dtoProperties)
                {
                    if (prop.Name == "IdArticulo") continue; // ¡Excluye el ID!

                    var value = prop.GetValue(actualizarArticulosDto);
                    var entityType = _bd.Entry(articuloExistente).Entity.GetType();
                    var entityProp = entityType.GetProperty(prop.Name);

                    if (value != null) // Solo actualiza si el DTO tiene valor
                    {
                        if (entityProp != null) // ¡Solo actualiza si la propiedad existe!
                        {
                            _bd.Entry(articuloExistente).Property(prop.Name).CurrentValue = value;
                            _bd.Entry(articuloExistente).Property(prop.Name).IsModified = true;
                        }
                    }
                }
            }
            else
                return false;

            //if (articuloExistente != null)
            //    _bd.Entry(articuloExistente).CurrentValues.SetValues(articulo);

            //Guardar();

            //var propiedadesExistentes = _bd.ArticulosPropiedades
            //    .Where(ap => ap.IdArticulo == articulo.IdArticulo)
            //    .Select
            //    (
            //        app => new ArticulosPropiedades
            //        {
            //            IdArticulo = articulo.IdArticulo,
            //            IdPropiedad = app.IdPropiedad,
            //            PropiedadValor = actualizarArticulosDto.PropiedadValor
            //        }
            //    ).FirstOrDefault();

            //if (propiedadesExistentes != null)
            //    _bd.Entry(propiedadesExistentes).CurrentValues.SetValues(propiedadesExistentes);         

            return Guardar();
        }

        public IEnumerable<CatArticulosDto> BuscarClaveArticulo(string Clave)
        {

            IQueryable<CatArticulosDto> query = _bd.CatArticulos
                .Where(a => a.Clave == Clave)
                .Select(a => new CatArticulosDto
                {
                    IdArticulo = a.IdArticulo,
                    Clave = a.Clave,
                    Nombre = a.Nombre,
                    Importe = a.Importe,
                    Cantidad = a.Cantidad,
                    IdEstatus = a.IdEstatus,
                    FechaRegistro = a.FechaRegistro,
                    Activo = a.Activo,
                    PropiedadNombre = _bd.ArticulosPropiedades
                        .Where(ap => ap.IdArticulo == a.IdArticulo)
                        .Select(ar => _bd.CatPropiedades.FirstOrDefault(p => p.IdPropiedad == ar.IdPropiedad).PropiedadNombre)
                        .FirstOrDefault(),
                    PropiedadValor = _bd.ArticulosPropiedades.Where(ap => ap.IdArticulo == a.IdArticulo)
                        .Select(ar => ar.PropiedadValor)
                        .FirstOrDefault()
                });

             
            if (!string.IsNullOrEmpty(Clave))            
                query = query.Where(a => a.Clave.ToLower().Contains(Clave.ToLower()));

            return query.ToList();
        }

        public bool CrearArticulo(CatArticulos articulo, string nombreUsuario, CrearArticuloDto crearArticuloDto)
        {
            _bd.CatArticulos.Add(articulo);
            Guardar();

            //if(crearArticuloDto.PropiedadValor != null && crearArticuloDto.PropiedadNombre != null)
            //{
            //    CatPropiedades catPropiedades = new CatPropiedades()
            //    {
            //        PropiedadNombre = crearArticuloDto.PropiedadNombre
            //    };

            //    _bd.CatPropiedades.Add(catPropiedades);
            //    Guardar();

            //    ArticulosPropiedades propiedad = new ArticulosPropiedades()
            //    {
            //        IdPropiedad = catPropiedades.IdPropiedad,
            //        PropiedadValor = crearArticuloDto.PropiedadValor,
            //        IdArticulo = articulo.IdArticulo
            //    };
            //    _bd.ArticulosPropiedades.Add(propiedad);
            //    Guardar();
            //}

            var idUsuario = _bd.Usuarios.FirstOrDefault(u => u.NombreUsuario.ToUpper() == nombreUsuario.ToUpper()).IdUsuario;

            Movimientos movimiento = new Movimientos()
            {
                Cantidad = articulo.Cantidad,
                IdTipoTransaccion = 4, // Asumiendo que 4 es para ingreso
                IdEstatus = 7, // Asumiendo que 7 es para finalizado
                Observaciones = "Ingreso inicial del artículo",
                IdUsuario = idUsuario
            };
            _bd.Movimientos.Add(movimiento);
            Guardar();

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

        public CatArticulosDto ObtenerArticulo(int idArticulo)
        {
            var catArticulosDto = _bd.CatArticulos
                .Where(a => a.IdArticulo == idArticulo)
                .Select(a => new CatArticulosDto
                {
                    IdArticulo = a.IdArticulo,
                    Clave = a.Clave,
                    Nombre = a.Nombre,
                    Importe = a.Importe,
                    Cantidad = a.Cantidad,
                    IdEstatus = a.IdEstatus,
                    FechaRegistro = a.FechaRegistro,
                    Activo = a.Activo,
                    PropiedadNombre = _bd.ArticulosPropiedades
                        .Where(ap => ap.IdArticulo == a.IdArticulo)
                        .Select(ar => _bd.CatPropiedades.FirstOrDefault(p => p.IdPropiedad == ar.IdPropiedad).PropiedadNombre)
                        .FirstOrDefault(),
                    PropiedadValor = _bd.ArticulosPropiedades.Where(ap => ap.IdArticulo == a.IdArticulo)
                        .Select(ar => ar.PropiedadValor)
                        .FirstOrDefault()
                }).FirstOrDefault();

            return catArticulosDto;
        }

        public CatArticulos ObtenerArticulo(string nombreArticulo)
        {
            return _bd.CatArticulos.FirstOrDefault(a => a.Nombre.ToUpper() == nombreArticulo.ToUpper());
        }

        public ICollection<CatArticulosDto> ObtenerArticulos()
        {
            return _bd.CatArticulos
                .Where(a => a.Activo == true)
                .Select(a => new CatArticulosDto
                {
                    IdArticulo = a.IdArticulo,
                    Clave = a.Clave,
                    Nombre = a.Nombre,
                    Importe = a.Importe,
                    Cantidad = a.Cantidad,
                    IdEstatus = a.IdEstatus,
                    FechaRegistro = a.FechaRegistro,
                    Activo = a.Activo,
                    PropiedadNombre = _bd.ArticulosPropiedades
                        .Where(ap => ap.IdArticulo == a.IdArticulo)
                        .Select(ar => _bd.CatPropiedades.FirstOrDefault(p => p.IdPropiedad == ar.IdPropiedad).PropiedadNombre)
                        .FirstOrDefault(),
                    PropiedadValor = _bd.ArticulosPropiedades.Where(ap => ap.IdArticulo == a.IdArticulo)
                        .Select(ar => ar.PropiedadValor)
                        .FirstOrDefault()
                }).ToList();
        }

        public bool CrearPropiedadArticulo(CrearPropiedadArticuloDto crearPropiedadArticuloDto)
        {
            var articulo = _bd.CatArticulos.Find(crearPropiedadArticuloDto.IdArticulo);
            if (articulo == null)            
                return false; // El artículo no existe
            else
            {
                var propiedades = new CatPropiedades
                {
                    PropiedadNombre = crearPropiedadArticuloDto.PropiedadNombre,
                    Activo = true
                };

                _bd.CatPropiedades.Add(propiedades);
                Guardar();

                var articulosPropiedades = new ArticulosPropiedades
                {
                    IdArticulo = crearPropiedadArticuloDto.IdArticulo,
                    IdPropiedad = propiedades.IdPropiedad,
                    PropiedadValor = crearPropiedadArticuloDto.PropiedadValor
                };
                _bd.ArticulosPropiedades.Add(articulosPropiedades);

            }

            return Guardar();
        }

        public ArticulosPropiedadesDto VerPropiedadesArticulo(int idArticulo)
        {
            return _bd.ArticulosPropiedades.Where(ap => ap.IdArticulo == idArticulo)
                .Select(ap => new ArticulosPropiedadesDto
                {
                    Articulo = _bd.CatArticulos.FirstOrDefault(a => a.IdArticulo == ap.IdArticulo).Nombre,
                    PropiedadNombre = _bd.CatPropiedades.FirstOrDefault(p => p.IdPropiedad == ap.IdPropiedad).PropiedadNombre,
                    PropiedadValor = ap.PropiedadValor,
                    Activo = ap.Activo
                }).FirstOrDefault();
        }

        public bool ActualizarPropiedadArticulo(ActualizarArticuloPropiedadDto actualizarArticuloPropiedadDto)
        {
            var propiedadExistente = _bd.ArticulosPropiedades
                .FirstOrDefault(ap => ap.IdArticulo == actualizarArticuloPropiedadDto.IdArticulo && ap.IdPropiedad == actualizarArticuloPropiedadDto.IdPropiedad);
            var dtoProperties = typeof(ActualizarArticuloPropiedadDto).GetProperties();

            if (propiedadExistente != null)
            {
                foreach (var prop in dtoProperties)
                {
                    if (prop.Name == "IdArticulo") continue; // ¡Excluye el ID!
                    if (prop.Name == "IdPropiedad") continue; // ¡Excluye el ID!

                    var value = prop.GetValue(actualizarArticuloPropiedadDto);
                    var entityType = _bd.Entry(propiedadExistente).Entity.GetType();
                    var entityProp = entityType.GetProperty(prop.Name);

                    if (value != null) // Solo actualiza si el DTO tiene valor
                    {
                        if (entityProp != null) // ¡Solo actualiza si la propiedad existe!
                        {
                            _bd.Entry(propiedadExistente).Property(prop.Name).CurrentValue = value;
                            _bd.Entry(propiedadExistente).Property(prop.Name).IsModified = true;
                        }
                    }
                }
                return Guardar();
            }
            else
                return false; // La propiedad no existe para el artículo especificado
            
        }

        public bool ActualizarPropiedadArticuloNombre(ActualizarNombrePropiedadDto actualizarNombrePropiedadDto)
        {
            var propiedadExistente = _bd.CatPropiedades
                .FirstOrDefault(p => p.IdPropiedad == actualizarNombrePropiedadDto.IdPropiedad);
            var dtoProperties = typeof(ActualizarNombrePropiedadDto).GetProperties();
            if (propiedadExistente != null)
            {
                foreach (var prop in dtoProperties)
                {
                    if (prop.Name == "IdPropiedad") continue; // ¡Excluye el ID!

                    var value = prop.GetValue(actualizarNombrePropiedadDto);
                    var entityType = _bd.Entry(propiedadExistente).Entity.GetType();
                    var entityProp = entityType.GetProperty(prop.Name);

                    if (value != null) // Solo actualiza si el DTO tiene valor
                    {
                        if (entityProp != null) // ¡Solo actualiza si la propiedad existe!
                        {
                            _bd.Entry(propiedadExistente).Property(prop.Name).CurrentValue = value;
                            _bd.Entry(propiedadExistente).Property(prop.Name).IsModified = true;
                        }
                    }
                }
                return Guardar();
            }
            else
                return false;
            
        }

        public bool EliminarPropiedadArticulo(EliminarPropiedadArticuloDto eliminarPropiedadArticuloDto)
        {
            var propiedadExistente = _bd.ArticulosPropiedades
                .FirstOrDefault(p => p.IdPropiedad == eliminarPropiedadArticuloDto.IdPropiedad &&
                p.IdArticulo == eliminarPropiedadArticuloDto.IdArticulo);
            var dtoProperties = typeof(EliminarPropiedadArticuloDto).GetProperties();

            if (propiedadExistente != null)
            {
                propiedadExistente.Activo = 0; // Marcar como inactivo
                foreach (var prop in dtoProperties)
                {
                    if (prop.Name == "IdArticulo") continue; // ¡Excluye el ID!
                    if (prop.Name == "IdPropiedad") continue; // ¡Excluye el ID!

                    var value = prop.GetValue(eliminarPropiedadArticuloDto);
                    var entityType = _bd.Entry(propiedadExistente).Entity.GetType();
                    var entityProp = entityType.GetProperty(prop.Name);

                    if (value != null) // Solo actualiza si el DTO tiene valor
                    {
                        if (entityProp != null) // ¡Solo actualiza si la propiedad existe!
                        {
                            _bd.Entry(propiedadExistente).Property(prop.Name).CurrentValue = value;
                            _bd.Entry(propiedadExistente).Property(prop.Name).IsModified = true;
                        }
                    }
                }

                return Guardar();
            }
            else
                return false;
        }

        public bool EliminarPropiedad(EliminarPropiedadDto eliminarPropiedadDto)
        {
            var propiedadExistente = _bd.CatPropiedades
                .FirstOrDefault(p => p.IdPropiedad == eliminarPropiedadDto.IdPropiedad);
            var dtoProperties = typeof(EliminarPropiedadDto).GetProperties();
            if(propiedadExistente != null)
            {
                propiedadExistente.Activo = false; // Marcar como inactivo
                foreach (var prop in dtoProperties)
                {
                    
                    if (prop.Name == "IdPropiedad") continue; // ¡Excluye el ID!

                    var value = prop.GetValue(eliminarPropiedadDto);
                    var entityType = _bd.Entry(propiedadExistente).Entity.GetType();
                    var entityProp = entityType.GetProperty(prop.Name);

                    if (value != null) // Solo actualiza si el DTO tiene valor
                    {
                        if (entityProp != null) // ¡Solo actualiza si la propiedad existe!
                        {
                            _bd.Entry(propiedadExistente).Property(prop.Name).CurrentValue = value;
                            _bd.Entry(propiedadExistente).Property(prop.Name).IsModified = true;
                        }
                    }
                }
                dtoProperties = null;
                
                var articuloPropiedadExistente = _bd.ArticulosPropiedades
                    .FirstOrDefault(ap => ap.IdPropiedad == propiedadExistente.IdPropiedad);   
                
                if(articuloPropiedadExistente != null)
                {
                    var eliminarArticuloPropiedadDto = new EliminarPropiedadArticuloDto
                    {
                        IdArticulo = articuloPropiedadExistente.IdArticulo,
                        IdPropiedad = articuloPropiedadExistente.IdPropiedad
                    };
                    dtoProperties = typeof(EliminarPropiedadArticuloDto).GetProperties();
                    articuloPropiedadExistente.Activo = 0; // Marcar como inactivo

                    foreach (var prop in dtoProperties)
                    {
                        if (prop.Name == "IdArticulo") continue; // ¡Excluye el ID!
                        if (prop.Name == "IdPropiedad") continue; // ¡Excluye el ID!

                        var value = prop.GetValue(eliminarArticuloPropiedadDto);
                        var entityType = _bd.Entry(articuloPropiedadExistente).Entity.GetType();
                        var entityProp = entityType.GetProperty(prop.Name);

                        if (value != null) // Solo actualiza si el DTO tiene valor
                        {
                            if (entityProp != null) // ¡Solo actualiza si la propiedad existe!
                            {
                                _bd.Entry(articuloPropiedadExistente).Property(prop.Name).CurrentValue = value;
                                _bd.Entry(articuloPropiedadExistente).Property(prop.Name).IsModified = true;
                            }
                        }
                    }
                }
                return Guardar();
            }
            else
                return false;
        }

        public bool ExistePropiedad(string nombrePropiedad)
        {
            bool valor = _bd.CatPropiedades
                .Any(p => p.PropiedadNombre.ToUpper() == nombrePropiedad.ToUpper() && p.Activo == true);
            return valor;
        }

        public bool ExistePropiedadArticulo(int IdArticulo, int IdPropiedad)
        {
            bool valor = _bd.ArticulosPropiedades
                .Any(ap => ap.IdArticulo == IdArticulo && ap.IdPropiedad == IdPropiedad && ap.Activo == 1);
            return valor;
        }

        public bool ExistePropiedad(int idPropiedad)
        {
            bool valor = _bd.CatPropiedades
                .Any(p => p.IdPropiedad == idPropiedad && p.Activo == true);
            return valor;
        }

        public bool ArticuloEntrada(ArticulosEntradas articuloEntrada, ArticuloEntradaDto articuloEntradaDto, string nombreUsuario)
        {
            var idUsuario = _bd.Usuarios.FirstOrDefault(u => u.NombreUsuario.ToUpper() == nombreUsuario.ToUpper()).IdUsuario;

            Movimientos movimiento = new Movimientos()
            {
                Cantidad = articuloEntradaDto.ArticuloEntradaCantidad,
                IdTipoTransaccion = 4, // Asumiendo que 1 es para ingreso
                IdEstatus = 7, // Asumiendo que 7 es para finalizado
                Observaciones = "Entrada de artículo",
                IdUsuario = idUsuario
            };
            _bd.Movimientos.Add(movimiento);
            Guardar();

            articuloEntrada.IdMovimiento = movimiento.IdMovimiento;
            _bd.ArticulosEntradas.Add(articuloEntrada);
            Guardar();            

            MovimientosDetalle movimientosDetalle = new MovimientosDetalle()
            {
                Cantidad = articuloEntradaDto.ArticuloEntradaCantidad,
                IdArticulo = articuloEntradaDto.IdArticulo,
                IdMovimiento = movimiento.IdMovimiento,
                IdEstatus = 7 // Asumiendo que 7 es para finalizado
            };
            _bd.MovimientosDetalle.Add(movimientosDetalle);
            Guardar();

            var articulo = _bd.CatArticulos.Find(articuloEntradaDto.IdArticulo);
            articulo.Cantidad += articuloEntradaDto.ArticuloEntradaCantidad;
            _bd.Entry(articulo).State = EntityState.Modified;
            _bd.Entry(articulo).Property(a => a.Cantidad).IsModified = true;

            return Guardar();
        }

        public bool ExisteArticulo(int idArticulo)
        {
            bool valor = _bd.CatArticulos
                .Any(a => a.IdArticulo == idArticulo && a.Activo == true);
            return valor;
        }
    }   
}
