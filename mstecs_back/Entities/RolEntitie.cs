using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace mstecs_back.Entities
{
    public class RolEntitie
    {
        [Key]
        public int id_rol { get; set; }
        public string rol { get; set; }
    }
}
