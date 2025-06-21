using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;

namespace LlantasGuerreroApi.Datos
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class AtritutoValidarRolJwt : TypeFilterAttribute
    {
        public AtritutoValidarRolJwt(int IdRol) : base(typeof(AtritutoValidarRolJwtFiltro))
        {
            Arguments = new object[] { IdRol };
        }
        
    }
    public class AtritutoValidarRolJwtFiltro : IAuthorizationFilter
    {
        private readonly int _rolIdRequerido;

        public AtritutoValidarRolJwtFiltro(int rolIdRequerido)
        {
            _rolIdRequerido = rolIdRequerido;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // 1. Obtener el claim "rol" del token JWT
            var rolClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "rol");

            if (rolClaim == null || !int.TryParse(rolClaim.Value, out int rolIdUsuario))
            {
                context.Result = new ForbidResult(); // 403
                return;
            }

            // 2. Validar si el rol del usuario coincide con el requerido
            if (rolIdUsuario != _rolIdRequerido)
            {
                context.Result = new ForbidResult(); // 403
            }
        }
    }
}
