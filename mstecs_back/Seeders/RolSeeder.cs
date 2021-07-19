using Microsoft.EntityFrameworkCore;
using mstecs_back.Entities;

namespace mstecs_back.Seeders
{
    public static class RolSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolEntitie>()
                .HasData(
                   new RolEntitie
                   {
                       id_rol = 1,
                       rol = "Administrador"
                   },
                   new RolEntitie
                   {
                       id_rol = 2,
                       rol = "Usuario"
                   }
            );
        }
    }
}
