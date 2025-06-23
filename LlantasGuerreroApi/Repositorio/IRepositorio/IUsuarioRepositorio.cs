using LlantasGuerreroApi.Modelos;
using LlantasGuerreroApi.Modelos.Dtos;

namespace LlantasGuerreroApi.Repositorio.IRepositorio
{
    public interface IUsuarioRepositorio
    {
        ICollection<UsuarioDto> ObtenerUsuarios();
        UsuarioDto ObtenerUsuario(int IdUsuario);
        IEnumerable<Usuarios> ObtenerUsuario(string NombreUsuario);
        bool ExisteUsuario(string nombreUsuario);
        bool ExisteUsuario(int IdUsuario);
        Task<LoginUsuarioRespuestaDto> IniciarSesion(LoginUsuarioDto loginUsuarioDto);
        Task<Usuarios> CrearUsuario(CrearUsuarioDto crearUsuarioDto);
        int ObtenerRolUsuario(int idUsuario);
        int ObtenerRolUsuario(string NombreUsuario);
    }
}
