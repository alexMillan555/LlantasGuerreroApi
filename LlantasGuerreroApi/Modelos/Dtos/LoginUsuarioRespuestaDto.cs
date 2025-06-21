namespace LlantasGuerreroApi.Modelos.Dtos
{
    public class LoginUsuarioRespuestaDto
    {
        public UsuarioDto UsuarioDto { get; set; }
        public int IdRol { get; set; }
        public string Token { get; set; }
    }
}
