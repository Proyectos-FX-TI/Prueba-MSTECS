using System.ComponentModel.DataAnnotations;

namespace mstecs_back.Entities
{
    public class UsuarioEntitie
    {
        [Key]
        public int id_usuario { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public byte[] password_hash { get; set; }
        public byte[] password_salt { get; set; }
        public int id_rol { get; set; }
    }
}