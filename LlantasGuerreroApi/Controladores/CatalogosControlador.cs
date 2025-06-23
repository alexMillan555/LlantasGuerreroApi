using AutoMapper;
using LlantasGuerreroApi.Modelos;
using LlantasGuerreroApi.Modelos.Dtos;
using LlantasGuerreroApi.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LlantasGuerreroApi.Controladores
{
    public class CatalogosControlador : ControllerBase
    {
        private readonly ICatalogoRepositorio _catalogoRepo;
        protected RespuestaAPI _respuestaApi;
        private readonly IMapper _mapper;

        public CatalogosControlador(ICatalogoRepositorio catalogoRepo, IMapper mapper)
        {
            _catalogoRepo = catalogoRepo;
            _mapper = mapper;
            this._respuestaApi = new();
        }

        [HttpGet("ObtenerRoles")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public IActionResult ObtenerRoles()
        {
            var idRol = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (idRol != "1" && idRol != "2" && idRol != "3")
            {
                return Unauthorized(); // o return Unauthorized();
            }

            var listaRoles =_catalogoRepo.ObtenerRoles();

            var listaRolesDto = new List<CatRolesDto>();

            foreach (var lista in listaRoles)
            {
                listaRolesDto.Add(_mapper.Map<CatRolesDto>(lista));
            }
            return Ok(listaRolesDto);
        }

    }
}
