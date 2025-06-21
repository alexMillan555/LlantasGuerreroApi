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

    }
}
