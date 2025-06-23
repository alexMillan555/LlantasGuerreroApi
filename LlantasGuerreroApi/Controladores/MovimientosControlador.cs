using AutoMapper;
using LlantasGuerreroApi.Modelos;
using LlantasGuerreroApi.Modelos.Dtos;
using LlantasGuerreroApi.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LlantasGuerreroApi.Controladores
{
    public class MovimientosControlador : ControllerBase
    {
        private readonly IMovimientoRepositorio _movRepo;
        private readonly IArticuloRepositorio _artRepo;
        protected RespuestaAPI _respuestaApi;
        private readonly IMapper _mapper;

        public MovimientosControlador(IMovimientoRepositorio movRepo, IArticuloRepositorio artRepo, IMapper mapper)
        {
            _movRepo = movRepo;
            _artRepo = artRepo;
            _mapper = mapper;
            this._respuestaApi = new();
        }

        [HttpGet("ObtenerListaMovimientos")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public IActionResult ObtenerListaMovimientos()
        {
            var idRol = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (idRol != "1" && idRol != "2" && idRol != "3")
            {
                return Unauthorized(); // o return Unauthorized();
            }

            var listaMovimientos = _movRepo.ObtenerMovimientos();

            var listMovimientosDto = new List<MovimientosDto>();


            foreach (var lista in listaMovimientos)
            {
                listMovimientosDto.Add(_mapper.Map<MovimientosDto>(lista));
            }
            return Ok(listMovimientosDto);
        }

        [HttpGet("{movimientoId:int}/ObtenerMovimiento", Name = "ObtenerMovimiento")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ObtenerMovimiento(int movimientoId)
        {
            var itemMovimiento = _movRepo.ObtenerMovimiento(movimientoId);

            if (itemMovimiento == null)
            {
                return NotFound();
            }

            var itemMovimientoDto = _mapper.Map<MovimientosDto>(itemMovimiento);

            return Ok(itemMovimientoDto);
        }

        [HttpPost("MovimientoVentas")]
        [ProducesResponseType(201, Type = typeof(MovimientosDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public IActionResult MovimientoVentas([FromBody] MovimientoVentaDto movimientoVentaDto)
        {
            var idRol = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (idRol != "1" && idRol != "2" && idRol != "3")
                return Unauthorized(); // o return Unauthorized();

            var usuario = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (movimientoVentaDto == null)
            {
                return BadRequest(ModelState);
            }

            var movimientoVenta = _mapper.Map<Movimientos>(movimientoVentaDto);
            if (_artRepo.ExisteArticulo(movimientoVentaDto.IdArticulo))
            {                

                if (!_movRepo.MovimientoVenta(movimientoVentaDto, usuario))
                {
                    ModelState.AddModelError("", $"Algo salio mal guardando el registro{movimientoVentaDto.IdArticulo}");
                    return StatusCode(404, ModelState);
                }

            }
            else
            {
                ModelState.AddModelError("", "El artículo no existe o está inactivo");
                return StatusCode(404, ModelState);
            }

            return CreatedAtRoute("ObtenerMovimiento", new { movimientoId = movimientoVenta }, movimientoVenta);
        }

        [HttpGet("ObtenerVentas")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public IActionResult ObtenerVentas([FromQuery] MovimientosVentaFiltro movimientosVentaFiltro)
        {
            var idRol = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (idRol != "1" && idRol != "2" && idRol != "3")
            {
                return Unauthorized(); // o return Unauthorized();
            }

            var listaMovimientosVentasFiltroDto = _movRepo.ObtenerVentas(movimientosVentaFiltro.NombreUsuario, movimientosVentaFiltro.IdArticulo, movimientosVentaFiltro.Fecha);

            var listaMovimientos = new List<Movimientos>();


            foreach (var lista in listaMovimientos)
            {
                listaMovimientosVentasFiltroDto.Add(_mapper.Map<MovimientosVentasDetalleDto>(lista));
            }
            return Ok(listaMovimientosVentasFiltroDto);
        }

    }
}
