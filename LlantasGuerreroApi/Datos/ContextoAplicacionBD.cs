using LlantasGuerreroApi.Modelos;
using Microsoft.EntityFrameworkCore;

namespace LlantasGuerreroApi.Datos
{
    public class ContextoAplicacionBD : DbContext
    {
        public ContextoAplicacionBD(DbContextOptions<ContextoAplicacionBD> opciones) : base(opciones)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        //Aquí puedes definir tus DbSets para las entidades
        public DbSet<ArticulosEntradas> ArticulosEntradas { get; set; }
        public DbSet<ArticulosPropiedades> ArticulosPropiedades { get; set; }
        public DbSet<CatArticulos> CatArticulos { get; set; }
        public DbSet<CatEstatus> CatEstatus { get; set; }
        public DbSet<CatPropiedades> CatPropiedades { get; set; }
        public DbSet<CatRoles> CatRoles { get; set; }
        public DbSet<CatTipoTransaccion> CatTipoTransaccion { get; set; }        
        public DbSet<Movimientos> Movimientos { get; set; }        
        public DbSet<MovimientosDetalle> MovimientosDetalle { get; set; }        
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<UsuarioRol> UsuarioRol { get; set; }

        // Otros métodos y propiedades del contexto pueden ir aquí
    }
}
