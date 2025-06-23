using AutoMapper;
using LlantasGuerreroApi.Modelos;
using LlantasGuerreroApi.Modelos.Dtos;
using LlantasGuerreroApi.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LlantasGuerreroApi.Controladores
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuarioControlador : ControllerBase
    {
        private readonly IUsuarioRepositorio _usRepo;
        protected RespuestaAPI _respuestaApi;
        private readonly IMapper _mapper;

        public UsuarioControlador(IUsuarioRepositorio usRepo, IMapper mapper)
        {
            _usRepo = usRepo;
            _mapper = mapper;
            this._respuestaApi = new();
        }

        [HttpGet("ObtenerListaUsuarios")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public IActionResult ObtenerListaUsuarios()
        {
            var idRol = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (idRol != "1" && idRol != "2")
            {
                return Unauthorized(); // o return Unauthorized();
            }

            var listaUsuarios = _usRepo.ObtenerUsuarios();

            var listaUsuariosDto = new List<UsuarioDto>();

            foreach (var lista in listaUsuarios)
            {
                listaUsuariosDto.Add(_mapper.Map<UsuarioDto>(lista));
            }
            return Ok(listaUsuariosDto);
        }

        [HttpGet("{usuarioId:int}/ObtenerUsuarioId", Name = "ObtenerUsuarioId")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public IActionResult ObtenerUsuarioId(int usuarioId)
        {
            var idRol = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (idRol != "1" && idRol != "2")
            {
                return Unauthorized(); // o return Unauthorized();
            }

            var itemUsuario = _usRepo.ObtenerUsuario(usuarioId);

            if (itemUsuario == null)
            {
                return NotFound();
            }

            var itemUsuarioDto = _mapper.Map<UsuarioDto>(itemUsuario);

            return Ok(itemUsuarioDto);
        }

        [HttpGet("ObtenerUsuarioNombre", Name = "ObtenerUsuarioNombre")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public IActionResult ObtenerUsuarioNombre(string nombreUsuario)
        {
            var idRol = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (idRol != "1" && idRol != "2")
            {
                return Unauthorized(); // o return Unauthorized();
            }

            try
            {
                var resultado = _usRepo.ObtenerUsuario(nombreUsuario);
                if (resultado.Any())
                {
                    return Ok(resultado);
                }

                return NotFound();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos de la aplicación");
            }
        }

        [HttpPost("registro")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Registro([FromBody] CrearUsuarioDto usuarioRegistroDto)
        {
            if(usuarioRegistroDto.IdRol !=4 && usuarioRegistroDto.IdRol != 6)
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessages.Add("Solo se permiten crear roles de usuario o cliente");
                return BadRequest(_respuestaApi);
            }

            bool validarNombreUsuarioUnico = _usRepo.ExisteUsuario(usuarioRegistroDto.NombreUsuario);
            if (!validarNombreUsuarioUnico)
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessages.Add("El nombre de usuario ya existe");
                return BadRequest(_respuestaApi);
            }

            var usuario = await _usRepo.CrearUsuario(usuarioRegistroDto);
            if (usuario == null)
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessages.Add("Error en el registro");
                return BadRequest(_respuestaApi);
            }

            _respuestaApi.StatusCode = HttpStatusCode.OK;
            _respuestaApi.IsSuccess = true;
            return Ok(_respuestaApi);

        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginUsuarioDto usuarioLoginDto)
        {

            var respuestaLogin = await _usRepo.IniciarSesion(usuarioLoginDto);

            if (respuestaLogin.UsuarioDto == null || string.IsNullOrEmpty(respuestaLogin.Token))
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessages.Add("El nombre de usuario o password son incorrectos");
                return BadRequest(_respuestaApi);
            }

            _respuestaApi.StatusCode = HttpStatusCode.OK;
            _respuestaApi.IsSuccess = true;
            _respuestaApi.Result = respuestaLogin;
            return Ok(_respuestaApi);
        }

    }
}
