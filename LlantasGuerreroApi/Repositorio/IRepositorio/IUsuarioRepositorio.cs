using LlantasGuerreroApi.Modelos;
using LlantasGuerreroApi.Modelos.Dtos;

namespace LlantasGuerreroApi.Repositorio.IRepositorio
{
    public interface IUsuarioRepositorio
    {
        ICollection<Usuarios> ObtenerUsuarios();
        Usuarios ObtenerUsuario(int IdUsuario);
        Usuarios ObtenerUsuario(string NombreUsuario);
        bool ExisteUsuario(string nombreUsuario);
        Task<LoginUsuarioRespuestaDto> IniciarSesion(LoginUsuarioDto loginUsuarioDto);
        Task<Usuarios> CrearUsuario(CrearUsuarioDto crearUsuarioDto);
        int ObtenerRolUsuario(int idUsuario);
        int ObtenerRolUsuario(string NombreUsuario);
    }
}
