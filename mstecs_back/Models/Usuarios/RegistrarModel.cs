using System.ComponentModel.DataAnnotations;

namespace mstecs_back.Models.Usuarios
{
    public class RegistrarModel
    {
        [Required]
        public string nombre { get; set; }

        [Required]
        public string correo { get; set; }

        [Required]
        public string password { get; set; }
    }
}