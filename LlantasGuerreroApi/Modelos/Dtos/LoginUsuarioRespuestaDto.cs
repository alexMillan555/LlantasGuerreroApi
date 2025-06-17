namespace LlantasGuerreroApi.Modelos.Dtos
{
    public class LoginUsuarioRespuestaDto
    {
        public Usuarios Usuario { get; set; }
        public int IdRol { get; set; }
        public string Token { get; set; }
    }
}
