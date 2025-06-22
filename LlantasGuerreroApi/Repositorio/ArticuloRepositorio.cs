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
                IdTipoTransaccion = 1, // Asumiendo que 1 es para ingreso
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
    }   
}
