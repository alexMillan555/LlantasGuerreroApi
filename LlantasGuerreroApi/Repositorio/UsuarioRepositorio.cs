using AutoMapper;
using LlantasGuerreroApi.Datos;
using LlantasGuerreroApi.Modelos;
using LlantasGuerreroApi.Modelos.Dtos;
using LlantasGuerreroApi.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;

namespace LlantasGuerreroApi.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ContextoAplicacionBD _bd;
        private string claveSecreta;
        private readonly IMapper _mapper;

        public UsuarioRepositorio(ContextoAplicacionBD bd, IConfiguration config, IMapper mapper)
        {
            _bd = bd;
            claveSecreta = config.GetValue<string>("ApiSettings:Secreta");
            _mapper = mapper;
        }

        public ICollection<Usuarios> ObtenerUsuarios()
        {
            return _bd.Usuarios.OrderBy(u => u.NombreUsuario).ToList();
        }

        public Usuarios ObtenerUsuario(int IdUsuario)
        {
            return _bd.Usuarios.FirstOrDefault(u => u.IdUsuario == IdUsuario);
        }

        public Usuarios ObtenerUsuario(string NombreUsuario)
        {
            return _bd.Usuarios.FirstOrDefault(u => u.NombreUsuario == NombreUsuario);
        }

        public bool ExisteUsuario(string nombreUsuario)
        {
            var usuarioBd = _bd.Usuarios.FirstOrDefault(u => u.NombreUsuario.ToLower().Trim() == nombreUsuario.ToLower().Trim());
            if (usuarioBd == null)            
                return false;
            else 
                return true;
            
        }

        public async Task<LoginUsuarioRespuestaDto> IniciarSesion(LoginUsuarioDto loginUsuarioDto)
        {
            var contraseñaEncriptada = obtenermd5(loginUsuarioDto.Contraseña);

            var usuario = _bd.Usuarios.FirstOrDefault(
                u => u.NombreUsuario.ToLower() == loginUsuarioDto.NombreUsuario.ToLower()
                && u.Contraseña == contraseñaEncriptada);

            if (usuario != null)
            {
                return new LoginUsuarioRespuestaDto()
                {
                    Token = "",
                    Usuario = null
                };
            }

            var usuarioDatos = _bd.Usuarios.Where(
                u => u.NombreUsuario.ToLower() == loginUsuarioDto.NombreUsuario)
                .Select(u => new UsuarioDatosDto
                {
                    Id = u.IdUsuario,
                    NombreUsuario = u.NombreUsuario
                }).FirstOrDefault();

            var usuarioRol = _bd.UsuarioRol.FirstOrDefault(
                u => u.IdUsuario == usuarioDatos.Id);

            if(usuarioRol == null)
            {
                return new LoginUsuarioRespuestaDto()
                {
                    Token = "",
                    Usuario = null
                };
            }

            var manejadoToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario.ToString()),
                    new Claim(ClaimTypes.Role, usuarioRol.IdRol.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manejadoToken.CreateToken(tokenDescriptor);
            LoginUsuarioRespuestaDto loginUsuarioRespuesta = new LoginUsuarioRespuestaDto()
            {
                Token = manejadoToken.WriteToken(token),
                Usuario = usuario
            };

            return loginUsuarioRespuesta;
        }

        public Task<UsuarioDatosDto> CrearUsuario(CrearUsuarioDto crearUsuarioDto)
        {
            throw new NotImplementedException();
        }

        public int ObtenerRolUsuario(int idUsuario)
        {
            throw new NotImplementedException();
        }

        public int ObtenerRolUsuario(string NombreUsuario)
        {
            throw new NotImplementedException();
        }

        public static string obtenermd5(string valor)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++)
                resp += data[i].ToString("x2").ToLower();
            return resp;
        }

    }
}
