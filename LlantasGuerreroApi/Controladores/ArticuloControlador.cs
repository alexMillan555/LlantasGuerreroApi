using AutoMapper;
using LlantasGuerreroApi.Datos;
using LlantasGuerreroApi.Modelos;
using LlantasGuerreroApi.Modelos.Dtos;
using LlantasGuerreroApi.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LlantasGuerreroApi.Controladores
{
    [Route("api/articulos")]
    [ApiController]
    public class ArticuloControlador : ControllerBase
    {
        private readonly IArticuloRepositorio _artRepo;
        protected RespuestaAPI _respuestaApi;
        private readonly IMapper _mapper;

        public ArticuloControlador(IArticuloRepositorio artRepo, IMapper mapper)
        {
            _artRepo = artRepo;
            _mapper = mapper;
            this._respuestaApi = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public IActionResult ObtenerArticulos()
        {
            var idRol = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (idRol != "1" && idRol != "2" && idRol != "3")
            {
                return Unauthorized(); // o return Unauthorized();
            }            

            var listArticulos = _artRepo.ObtenerArticulos();

            var listArticulosDto = new List<CatArticulosDto>();
            

            foreach (var lista in listArticulos)
            {
                listArticulosDto.Add(_mapper.Map<CatArticulosDto>(lista));
            }
            return Ok(listArticulosDto);
        }

        [HttpGet("{articuloId:int}", Name = "ObtenerArticulo")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ObtenerArticulo(int articuloId)
        {
            var itemArticulo = _artRepo.ObtenerArticulo(articuloId);

            if (itemArticulo == null)
            {
                return NotFound();
            }

            var itemArticuloDto = _mapper.Map<CatArticulosDto>(itemArticulo);

            return Ok(itemArticuloDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CatArticulosDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public IActionResult CrearArticulo([FromBody] CrearArticuloDto crearArticuloDto)
        {
            var idRol = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (idRol != "1" && idRol != "2" && idRol != "3")            
                return Unauthorized(); // o return Unauthorized();
            
            var usuario = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (crearArticuloDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_artRepo.ExisteArticulo(crearArticuloDto.Nombre))
            {
                ModelState.AddModelError("", "El artículo ya existe");
                return StatusCode(404, ModelState);
            }

            var articulo = _mapper.Map<CatArticulos>(crearArticuloDto);

            if (!_artRepo.CrearArticulo(articulo, usuario, crearArticuloDto))
            {
                ModelState.AddModelError("", $"Algo salio mal guardando el registro{articulo.Nombre}");
                return StatusCode(404, ModelState);
            }

            return CreatedAtRoute("ObtenerArticulo", new { articuloId = articulo.IdArticulo }, articulo);
        }

        [HttpPatch("{articuloId:int}/Actualizar", Name = "ActualizarArticulo")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public IActionResult ActualizarArticulo(int articuloId, [FromBody] ActualizarArticulosDto actualizarArticulosDto)
        {
            var idRol = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (idRol != "1" && idRol != "2" && idRol != "3")
                return Unauthorized(); // o return Unauthorized();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (actualizarArticulosDto == null || articuloId != actualizarArticulosDto.IdArticulo)
            {
                return BadRequest(ModelState);
            }

            var peliculaExistente = _artRepo.ObtenerArticulo(articuloId);
            if (peliculaExistente == null)
            {
                return NotFound($"No se encontro el artículo con ID {articuloId}");
            }

            var articulo = _mapper.Map<CatArticulos>(actualizarArticulosDto);

            if (!_artRepo.ActualizarArticulo(articulo, actualizarArticulosDto))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el registro{articulo.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpPatch("{articuloId:int}/Baja", Name = "BajaArticulo")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public IActionResult BajaArticulo(int articuloId, [FromBody] BajaArticuloDto bajaArticuloDto)
        {
            var idRol = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (idRol != "1" && idRol != "2" && idRol != "3")
                return Unauthorized(); // o return Unauthorized();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (bajaArticuloDto == null || articuloId != bajaArticuloDto.IdArticulo)
            {
                return BadRequest(ModelState);
            }

            var peliculaExistente = _artRepo.ObtenerArticulo(articuloId);
            if (peliculaExistente == null)
            {
                return NotFound($"No se encontro el artículo con ID {articuloId}");
            }

            var articulo = _mapper.Map<CatArticulos>(bajaArticuloDto);

            if (!_artRepo.BajaArticulo(articulo, bajaArticuloDto))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el registro{articulo.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

    }
}
