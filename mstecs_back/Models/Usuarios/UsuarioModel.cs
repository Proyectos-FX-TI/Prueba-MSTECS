namespace mstecs_back.Models.Usuarios
{
    public class UsuarioModel
    {
        public int id_usuario { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public int id_rol { get; set; }
        public string rol { get; set; }
    }
}